window.DynamicTable = {
    changePageSize: function (size) {
        const params = new URLSearchParams(window.location.search);
        params.set("pageSize", size);
        params.set("page", 1);
        window.location.search = params.toString();
    },
    searchTable: function (term) {
        const params = new URLSearchParams(window.location.search);
        params.set("search", term);
        params.set("page", 1);
        window.location.search = params.toString();
    },
    goToPage: function (page) {
        const params = new URLSearchParams(window.location.search);
        params.set("page", page);
        window.location.search = params.toString();
    },
    sortBy: function (field) {
        const params = new URLSearchParams(window.location.search);
        const currentSort = params.get("sort") || "";
        const currentDir = params.get("dir") || "asc";
        const newDir = currentSort === field && currentDir === "asc" ? "desc" : "asc";
        params.set("sort", field);
        params.set("dir", newDir);
        window.location.search = params.toString();
    },
    abrirModalExcluir: function (id) {
        document.getElementById('inputDeleteId').value = id;
        const modal = new bootstrap.Modal(document.getElementById('modalConfirmDelete'));
        modal.show();
    }
};