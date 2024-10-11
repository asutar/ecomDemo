
function GetAllSubCategories() {
    var ddlId = '#ddlSubCategory';
    $.ajax({
        type: "GET",
        url: "./SellerProduct/GetAllSubCategories",
        dataType: 'json',
        success: function (response) {
            $(ddlId).empty();
            $(ddlId).append($('<option/>').val('0').text('Select'));

            $.each(response, function (i, item) {
                $(ddlId).append($('<option/>')
                    .val(item.SUB_CATEGORY_ID)
                    .text(item.SUB_CATEGORY_DESCRIPTION));
            });
        }
    });
}
function GetCategory() {

    var ddlId = '#CATEGORY_ID';
    $.ajax({
        type: "GET",
        url: "./SellerProduct/GetCategories",
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
        url: "./SellerProduct/GetCategories",
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
function OpenProductModal() {
    $('#ProductDetailsModal').modal('show');
}
// Load All Products
function LoadProducts() {
    $.ajax({
        url: './SellerProduct/GetAllProducts',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var table = $('#RptProductDetails').DataTable();
            table.clear();

            $.each(data, function (index, item) {
                table.row.add([
                    item.ProductId,
                    item.Title,
                    item.CATEGORY_DESCRIPTION,
                    item.SUB_CATEGORY_DESCRIPTION,
                    item.Brand,
                    item.SIZE_DESCRIPTION,
                    item.COLOR_DESCRIPTION,
                    item.GENDER_DESCRIPTION,
                    item.MRP,
                    item.Price,
                    '<button type="button" class="btn btn-primary btn-sm" onclick="EditProduct(' + item.ProductId + ')">Edit</button>' +
                    '<button type="button" class="btn btn-danger btn-sm" onclick="DeleteProduct(' + item.ProductId + ')">Delete</button>'
                ]);
            });

            table.draw();
        }
    });
}

// Open Edit Modal
function EditProduct(productId) {
    debugger;
    $.ajax({
        url: './SellerProduct/GetProductById',
        type: 'GET',
        data: { id: productId },
        success: function (response) {
            $('#txtProductTitle').val(response.Title);
            $('#txtDescription').val(response.Description);
            $('#ddlCategory').val(response.CategoryId);
            $('#ddlSubCategory').val(response.SubCategoryId);
            $('#txtBrand').val(response.Brand);
            $('#ddlSize').val(response.SizeId);
            $('#ddlColor').val(response.ColorId);
            $('#txtMRP').val(response.MRP);
            $('#txtPrice').val(response.Price);

            $('#ProductDetailsModal').modal('show');
        }
    });
}
function AddProduct() {
    // Collect values from the modal form
    var product = {
        Title: $('#txtProductTitle').val(),
        Description: $('#txtDescription').val(),
        CategoryId: $('#ddlCategory').val(),
        SubCategoryId: $('#ddlSubCategory').val(),
        Brand: $('#txtBrand').val(),
        SizeId: $('#ddlSize').val(),
        ColorId: $('#ddlColor').val(),
        MRP: $('#txtMRP').val(),
        Price: $('#txtPrice').val(),
        Gender_id: $('#ddlGender').val(),
    };

    $.ajax({
        type: "POST",
        url: "./SellerProduct/AddProduct",  // Make sure the URL is correct and maps to the appropriate action in the controller
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(product),  // Convert the object to a JSON string
        dataType: "json",
        success: function (response) {
            if (response.success) {
                alert("Product added successfully!");
                $('#ProductDetailsModal').modal('hide');
                LoadProducts();  // Refresh the product table after adding a new product
            } else {
                alert("Failed to add product. Please check the data and try again.");
            }
        },
        error: function (xhr, status, error) {
            console.log("Error:", error);
            console.log("XHR:", xhr.responseText);
            alert("An error occurred while adding the product.");
        }
    });
}
function DeleteProduct(productId) {
    if (confirm("Are you sure you want to delete this product?")) {
        $.ajax({
            url: './SellerProduct/DeleteProduct',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                if (response.success) {
                    alert("Product deleted successfully!");
                    // Refresh the product table after successful deletion
                    LoadProducts();
                } else {
                    alert("Failed to delete the product. Please try again.");
                }
            },
            error: function (xhr, status, error) {
                console.log("Error:", error);
                alert("An error occurred while deleting the product: " + xhr.responseText);
            }
        });
    }
}
