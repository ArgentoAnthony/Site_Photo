// Script pour basculer entre le mode d'édition et le mode d'affichage
$(document).ready(function () {
    $(".editBtn").click(function () {
        var categoryId = $(this).data("id");
        var categoryNameSpan = $("#categoryName_" + categoryId);
        var editCategoryNameInput = $("#editCategoryName_" + categoryId);
        var editBtn = $(this);

        if (editBtn.text() === "Edit") {
            // Switch to edit mode
            categoryNameSpan.hide();
            editCategoryNameInput.val(categoryNameSpan.text()).show();
            editBtn.text("Update");
        } else {
            // Switch to view mode and update category name
            var newName = editCategoryNameInput.val();
            $.post("/Home/UpdateCategory", { id: categoryId, newName: newName }, function () {
                categoryNameSpan.text(newName).show();
                editCategoryNameInput.hide();
                editBtn.text("Edit");
            });
        }
    });
});

