function GetAllSubCategories() {
    // Initialize the DataTable
    var table = $('#subCategoryTable').DataTable({
        "ajax": {
            "url": './SubCategoryMaster/GetAllSubCategories',
            "type": "GET",
            "datatype": "json",
            "dataSrc": "" // Use an empty dataSrc if your JSON response is a simple array
        },
        "columns": [
            { "data": "SUB_CATEGORY_ID" },
            { "data": "SUB_CATEGORY_DESCRIPTION" },
            { "data": "CATEGORY_DESCRIPTION" },
            { "data": "CATEGORY_ID" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return '<button id="btnEdit" class="btn btn-success btn-sm rounded-0" data-original-title="Edit" onclick="EditSubCategory(\'' + encodeURIComponent(row.SUB_CATEGORY_ID)

                        + '\',\'' + encodeURIComponent(row.SUB_CATEGORY_DESCRIPTION)
                        + '\',\'' + encodeURIComponent(row.CATEGORY_DESCRIPTION)
                        + '\',\'' + encodeURIComponent(row.CATEGORY_ID)
                + '\')"><i class="fa fa-edit"></i></button >';
                }
            }
        ]
    });
}
function EditSubCategory(SUB_CATEGORY_ID, SUB_CATEGORY_DESCRIPTION, CATEGORY_DESCRIPTION, CATEGORY_ID) {
    //debugger;
    //GetCategoryModal();
    $('#txtSubCategory').val(SUB_CATEGORY_DESCRIPTION);
    //var CatId = parseInt(CATEGORY_ID);
    $("#ddlCategory").val(CATEGORY_ID);
    //$('#ddlCategory').val(CatId).trigger();
    $('#hdnSubCategoryId').val(SUB_CATEGORY_ID);
    $('#SubCategoryModal').modal('show');
}
function GetCategory() {
  
    var ddlId = '#CATEGORY_ID';
    $.ajax({
        type: "GET",
        url: "./SubCategoryMaster/GetCategories",
        dataType: 'json',
        //async: true,
        success: function (response) {
            $(ddlId).empty();
            $(ddlId).append($('<option/>').val('0').text('Select'));

            $.each(response, function (i, item) {
                $(ddlId).append($('<option/>')
                    .val(item.Value)
                    .text(item.Text));
            });
        }
    });
};
function GetCategoryModal() {

    var ddlId = '#ddlCategory';
    $.ajax({
        type: "GET",
        url: "./SubCategoryMaster/GetCategories",
        dataType: 'json',
        //async: true,
        success: function (response) {
            $(ddlId).empty();
            $(ddlId).append($('<option/>').val('0').text('Select'));

            $.each(response, function (i, item) {
                //$(ddlId).append($('<option/>')
                //    .val(item.Value)
                //    .text(item.Text));
                $(ddlId).append($('<option data-RoleId = "' + item.Value + '"/>').val(item.Value).text(item.Text));
            });
        }
    });
};
function UpdateSubCategory() {

    $.ajax({
        url: './SubCategoryMaster/CreateOrUpdate',
        type: 'POST',
        data: {
            SUB_CATEGORY_ID: $('#hdnSubCategoryId').val(),
            SUB_CATEGORY_DESCRIPTION: $('#txtSubCategory').val(),
            CATEGORY_ID: $('#ddlCategory').val(),
        },
        success: function (response) {
            // Show success message
            alert('SubCategory updated successfully!');

            // Re-fetch the DataTable data
            $('#subCategoryTable').DataTable().ajax.reload();
        },
        error: function (xhr, status, error) {
            alert('Failed to update SubCategory. Error: ' + error);
        }
    });
}
function DissmissModal() {
    $('#SubCategoryModal').modal('hide');
}