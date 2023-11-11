﻿
$(function () {
    var tableRow;
    var rowData;

    function DeleteCategory(e) {

        var categoryid = $(e).attr("categoryid");

        var data = {
            "CATEGORYID": Number(categoryid)
        }

        CallRequest("/Category/Delete", data, function (rsp) {
            var tr = $(e).parent().parent();
            $(tr).remove();
            PrintCounts();
        });
    }

    $('.select2').select2({
        placeholder: 'Choose one',
        searchInputPlaceholder: 'Search'
    });

    $(document).on("click", ".btnNewSave", function (e) {
        e.preventDefault();
        //console.log(e);
        var formData = $('#frmNewCategoryFormData').serializeArray();

        if (formData[0].value == "") {
            swal("Alert", "You should enter the category name!", "error");
            return;
        }

        var data = {
            "CATEGORYNAME": formData[0].value,
            "PARENTCATEGORYID": formData[1].value,
        }

        CallRequest("/Category/Create", data, function (rsp) {

            $("#frmNewCategory").modal("hide");

            var PARENTCATEGORYNAME = rsp.Data.PARENTCATEGORYNAME ?? "";
            var CREATEDDATE = ParseDate(rsp.Data.CREATEDDATE);
            var CATEGORYID = rsp.Data.CATEGORYID;
            var COUNT = $("#tblCategory tr").length;

            var newRow = "<tr><td>" + COUNT + "</td><td>" + rsp.Data.CATEGORYNAME + "</td><td>" + PARENTCATEGORYNAME + "</td><td>" + CREATEDDATE + "</td>";
            newRow += '<td><button categoryid="' + CATEGORYID + '" type="button" class="btn btn-danger btnUpdate" style="color:white;"><i class="fa fa-edit"></i></button>&nbsp;'
            newRow += '<button categoryid="' + CATEGORYID + '" type="button" class="btn btn-danger btnDelete" style="color:white;"><i class="far fa-trash-alt"></i></button></td></tr>';

            var NoData = $.find("#NoData");
            if (NoData.length != 0) {
                $(NoData[0]).remove();
            }

            $('#tblCategory > tbody:last-child').append(newRow);

            $(document).on("click", ".btnDelete" + CATEGORYID, function (e) {
                DeleteCategory(e);
            });

            PrintCounts();

        });
    });

    $(document).on("click", ".btnEditSave", function (e) {
        e.preventDefault();

        var formData = $('#frmEditCategoryFormData').serializeArray();

        if (formData[0].value == "") {
            swal("Alert", "You should enter the category name!", "error");
            return;
        }

        if (formData[1] == undefined) {
            $(".cmbEditCategory").val(-1).trigger("change");

            formData.push({
                "name": "cmbEditCategory",
                "value": -1
            });

        }

        var data = {
            "CATEGORYID": rowData.CATEGORYID,
            "CATEGORYNAME": formData[0].value,
            "PARENTCATEGORYID": formData[1].value,
        }

        console.log(data);

        CallRequest("/Category/Update", data, function (rsp) {
            $(tableRow[1]).text(rsp.Data.CATEGORYNAME);
            $(tableRow[2]).text(rsp.Data.PARENTCATEGORYNAME);
            $("#frmEditCategory").modal("hide");
            PrintCounts();
        });
    });

    $(document).on("click", ".btnUpdate", function (e) {
        e.stopPropagation();
        rowData = JSON.parse($(this).attr("data"));
        //console.log(data);
        $("#frmEditCategory").modal("show");

        if (rowData.PARENTCATEGORYID == 0) {
            rowData.PARENTCATEGORYID = -1;
        }

        FillSelect2(".cmbEditCategory", function () {
            $(".txtEditCategoryName").val(rowData.CATEGORYNAME);
            $(".cmbEditCategory").val(rowData.PARENTCATEGORYID).trigger("change");
        });

        tableRow = $(this).parent().parent().children();
    });

    $(document).on("click", ".btnDelete", function (e) {
        e.stopPropagation();
        DeleteCategory(this);
    });

    $(document).on("click", ".btnNewCategory", function (e) {
        e.stopPropagation();
        $("#frmNewCategory").modal("show");
        FillSelect2(".cmbNewCategory");
    });

    $(document).on("keyup", ".txtCategorySearch", function () {

        var value = $(this).val().toLowerCase();

        if (!value) {
            $("#tblCategory tr").show();
            return;
        }

        $("#tblCategory tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });

    });
});

