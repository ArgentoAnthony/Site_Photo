//Script pour la confirmation de suppression de la catégorie
$(document).ready(function () {
    $(".deleteBtn").click(function (event) {
        var categoryId = $(this).data("id");
        var categoryName = $(this).data("name");
        if (confirm("Êtes-vous sûr de vouloir supprimer la catégorie '" + categoryName + "'? Toutes les photos associées seront également supprimées.")) {
            window.location.href = "/Home/DeleteCategory/" + categoryId;
        } else {
            event.preventDefault();
        }
    });
});