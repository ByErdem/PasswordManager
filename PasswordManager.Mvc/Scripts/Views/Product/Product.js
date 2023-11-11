$(function () {

    var IMAGEBASE64 = "";

    $('.select2').select2({
        placeholder: 'Choose one',
        searchInputPlaceholder: 'Search'
    });

    function DeleteProduct(e) {

        var productid = $(e).attr("productid");

        var data = {
            "PRODUCTID": Number(productid)
        }

        CallRequest("/Product/Delete", data, null, true, function (rsp) {
            var tr = $(e).parent().parent();
            $(tr).remove();
            PrintCounts();
        });
    }

    $(document).on("click", ".btnUpdate", function (e) {
        e.preventDefault();
    });

    $(document).on("click", ".btnDelete", function (e) {
        e.preventDefault();
        DeleteProduct(this);
    });

    $(document).on("click", ".btnNewProduct", function (e) {
        e.preventDefault();
        console.log("New Product")
        $("#frmNewProduct").modal("show");
        FillSelect2WithMainCategories(".cmbMainCategory", undefined, function (data) {

            var data = {
                "categoryId": Number(data.id)
            }

            CallRequest("/Category/GetSubCategories", data, null, true, function (rsp) {
                FillSelect2WithSubCategories(".cmbSubCategory", rsp.Data, undefined, function (data2) {
                    console.log(data2)
                });
            });

        });
    });

    $(document).on("click", ".btnProductSave", function () {
        var formData = $('#frmNewProductFormData').serializeArray();

        var data = {
            "PRODUCTNAME": formData[0].value,
            "CATEGORYID": formData[2].value,
            "PRICE": formData[3].value,
            "IMAGEBASE64": IMAGEBASE64.replace("data:image/png;base64,", "")
        }

        console.log(data);

        if (data.PRODUCTNAME == "" || data.PRICE == "" || data.IMAGEBASE64 == "" || data.CATEGORYID == "Seçiniz") {
            swal("Alert", "You should fill all fields!", "error");
            return;
        }


        CallRequest("/Product/Create", data, null, true, function (rsp) {

            if (rsp.ResultStatus == 0) {

                $("#frmNewProduct").modal("hide");

                var IMAGEPATH = window.baseUrl + "Images/" + rsp.Data.IMAGEPATH + "?t=" + UUIDV4();
                var PRODUCTNAME = rsp.Data.PRODUCTNAME ?? "";
                var PRICE = rsp.Data.PRICE;
                var CREATEDDATE = ParseDate(rsp.Data.CREATEDDATE);
                var CATEGORYID = rsp.Data.CATEGORYID;
                var CATEGORYNAME = rsp.Data.CATEGORYNAME;
                var PRODUCTID = rsp.Data.PRODUCTID;

                var COUNT = $("#tblCategory tr").length;
                if (COUNT == 0) {
                    COUNT = 1;
                }

                var newRow = '<tr><td>' + COUNT + '</td><td><img src="' + IMAGEPATH + '" /></td><td>' + PRODUCTNAME + '</td><td>' + CATEGORYNAME + '</td><td>' + PRICE + '</td><td>' + CREATEDDATE + '</td>';
                newRow += '<td><button productid="' + PRODUCTID + '" categoryid="' + CATEGORYID + '" type="button" class="btn btn-danger btnUpdate" style="color:white;"><i class="fa fa-edit"></i></button>&nbsp;'
                newRow += '<button productid="' + PRODUCTID + '" categoryid="' + CATEGORYID + '" type="button" class="btn btn-danger btnDelete" style="color:white;"><i class="far fa-trash-alt"></i></button></td></tr>';

                var NoData = $.find("#NoData1");
                if (NoData.length != 0) {
                    $(NoData[0]).remove();
                }

                $('#tblProduct > tbody:last-child').append(newRow);

                $(document).on("click", ".btnDelete" + CATEGORYID, function (e) {
                    DeleteProduct(e);
                });

                PrintCounts();
            }

        });

    });

    $(document).on('change', '.dropify', function (e) {

        var file = e.target.files[0]; // Seçilen dosyayı al

        if (file) {
            // Dosyayı okumak için yeni bir FileReader örneği oluştur
            var reader = new FileReader();

            // Dosyanın okunması tamamlandığında tetiklenecek olay
            reader.onload = function (readerEvt) {
                // Dosyanın base64 formatındaki içeriğini al
                IMAGEBASE64 = readerEvt.target.result;
                //console.log(base64); // Base64 çıktısını konsola yazdır
            };

            // Dosyanın içeriğini Base64 formatında oku
            reader.readAsDataURL(file);
        }


    });

});