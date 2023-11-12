$(function () {
    $(document).on("click", ".btnNewSave", function (e) {
        e.preventDefault();
        //console.log(e);
        var formData = $('#frmNewPasswordFormData').serialize();
        console.log(formData);

        //if (formData[0].value == "") {
        //    swal("Alert", "You should enter the category name!", "error");
        //    return;
        //}

        //var data = {
        //    "CATEGORYNAME": formData[0].value
        //}

        //CallRequest("/Category/Create", data, null, true, function (rsp) {

        //    $("#frmNewCategory").modal("hide");
        //    var CREATEDDATE = ParseDate(rsp.Data.CREATEDDATE);
        //    var CATEGORYID = rsp.Data.CATEGORYID;
        //    var COUNT = $("#tblCategory tr").length;

        //    var newRow = "<tr><td>" + COUNT + "</td><td>" + rsp.Data.CATEGORYNAME + "</td><td>" + CREATEDDATE + "</td>";
        //    newRow += '<td><button categoryid="' + CATEGORYID + '" type="button" class="btn btn-danger btnUpdate" style="color:white;"><i class="fa fa-edit"></i></button>&nbsp;'
        //    newRow += '<button categoryid="' + CATEGORYID + '" type="button" class="btn btn-danger btnDelete" style="color:white;"><i class="far fa-trash-alt"></i></button></td></tr>';

        //    var NoData = $.find("#NoData");
        //    if (NoData.length != 0) {
        //        $(NoData[0]).remove();
        //    }

        //    $('#tblCategory > tbody:last-child').append(newRow);

        //    $(document).on("click", ".btnDelete" + CATEGORYID, function (e) {
        //        DeleteCategory(e);
        //    });

        //    PrintCounts();

        //});
    });

    $(document).on("click", ".btnNewPassword", function (e) {
        e.stopPropagation();
        $("#frmNewPassword").modal("show");
        FillSelect2(".cmbCategory");
    });
});