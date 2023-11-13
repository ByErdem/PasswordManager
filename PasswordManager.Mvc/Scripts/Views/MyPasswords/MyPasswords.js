$(function () {

    $('.select2').select2({
        placeholder: 'Choose one',
        searchInputPlaceholder: 'Search'
    });

    function DeletePassword(e) {

        var passwordId = $(e).attr("passwordId");

        var data = {
            "ID": Number(passwordId)
        }

        var settings = {
            link: "/MyPasswords/Delete",
            data: data,
            object: null,
            tokenNeeded: true,
            event: function (rsp) {
                var tr = $(e).parent().parent();
                $(tr).remove();
            }
        }

        CallRequest(settings);
    }



    function GeneratePassword(length = 12) {

        if (length == undefined || length == null)
            length = 12;

        const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+~`|}{[]:;?><,./-=";
        let password = "";
        for (var i = 0, n = charset.length; i < length; ++i) {
            password += charset.charAt(Math.floor(Math.random() * n));
        }
        return password;
    }


    $(document).on("click", ".btnGeneratePassword", function (e) {
        e.preventDefault();
        var passwordLength = $(".txtPasswordLength").val();
        var password = GeneratePassword(passwordLength);
        $(".txtPassword").val(password);
    });

    $(document).on("click", ".btnNewSave", function (e) {
        e.preventDefault();

        var formElement = document.getElementById("frmNewPasswordFormData");
        var formData = new FormData(formElement);

        console.log(formData.get("cmbNewCategory"));
        if (formData.get("cmbNewCategory") == -1) {
            swal("Uyarı", "Lütfen bir kategori seçiniz!", "error");
            return;
        }

        var data = {
            "NAME": formData.get("txtPasswordName"),
            "URL": formData.get("txtUrl"),
            "CATEGORYID": formData.get("cmbNewCategory"),
            "USERNAME": formData.get("txtUsername"),
            "PASSWORD": formData.get("txtPassword"),

        }

        var settings = {
            link: "/MyPasswords/Create",
            data: data,
            object: null,
            tokenNeeded: true,
            event: function (rsp) {

                console.log(rsp);

                $("#frmNewPassword").modal("hide");
                var COUNT = $("#tblPasswords tr").length;
                var NAME = rsp.Data.NAME;
                var URL = rsp.Data.URL;
                var CATEGORYNAME = rsp.Data.CATEGORYNAME;
                var USERNAME = rsp.Data.USERNAME;
                var PASSWORD = rsp.Data.PASSWORD;

                var ID = rsp.Data.ID;
                var CATEGORYID = rsp.Data.CATEGORYID;

                var newRow = "<tr><td>" + COUNT + "</td><td>" + NAME + "</td><td>" + URL + "</td><td>" + USERNAME + "</td><td>" + PASSWORD + "</td><td>" + CATEGORYNAME + "</td>";
                newRow += '<td><button passwordId="' + ID + '" type="button" class="btn btn-danger btnDelete" style="color:white;"><i class="far fa-trash-alt"></i></button></td></tr>';

                //var newRow = "<tr><td>" + COUNT + "</td><td>" + NAME + "</td><td>" + URL + "</td><td>" + USERNAME + "</td><td>" + PASSWORD + "</td><td>" + CATEGORYNAME + "</td>";
                //newRow += '<td><button passwordId="' + ID + '" data="' + JSON.stringify(rsp.Data) + '" type="button" class="btn btn-danger btnUpdate" style="color:white;"><i class="fa fa-edit"></i></button>&nbsp;'
                //newRow += '<button passwordId="' + ID + '" type="button" class="btn btn-danger btnDelete" style="color:white;"><i class="far fa-trash-alt"></i></button></td></tr>';

                var NoData = $.find("#NoData");
                if (NoData.length != 0) {
                    $(NoData[0]).remove();
                }

                $('#tblPasswords > tbody:last-child').append(newRow);

                $(document).on("click", ".btnDelete" + CATEGORYID, function (e) {
                    DeletePassword(e);
                });
            }
        }

        CallRequest(settings);

    });

    $(document).on("click", ".btnDelete", function (e) {
        e.stopPropagation();
        DeletePassword(this);
    });


    $(document).on("click", ".btnNewPassword", function (e) {
        e.stopPropagation();
        $("#frmNewPassword").modal("show");
/*        FillSelect2(".cmbNewCategory");*/
    });

    $(document).on("keyup", ".txtPasswordSearch", function () {

        var value = $(this).val().toLowerCase();

        if (!value) {
            $("#tblPasswords tr").show();
            return;
        }

        $("#tblPasswords tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });

    });


});