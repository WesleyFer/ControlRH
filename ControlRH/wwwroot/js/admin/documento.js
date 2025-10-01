window.Documento = {

    init: function () {
        $(document).ready(function () {
            // dispara quando muda a carteira
            $("#CarteiraClienteId").on("change", function () {
                var carteiraId = $(this).val();

                if (!carteiraId || carteiraId === "00000000-0000-0000-0000-000000000000") {
                    // limpa colaboradores se não tiver carteira válida
                    Documento.limparColaboradores();
                    return;
                }

                Documento.carregarColaboradores(carteiraId);
            });
        });
    },

    carregarColaboradores: function (carteiraId) {
        $.getJSON(`/Admin/Documento/ColaboradoresPorCarteira`, { carteiraClienteId: carteiraId })
            .done(function (data) {
                var $select = $("#ColaboradorId");
                $select.empty();
                $select.append($("<option>").val("").text("-- Selecione --"));

                $.each(data, function (i, colaborador) {
                    $select.append($("<option>").val(colaborador.id).text(colaborador.nome));
                });
            })
            .fail(function () {
                alert("Erro ao carregar colaboradores.");
            });
    },

    limparColaboradores: function () {
        var $select = $("#ColaboradorId");
        $select.empty();
        $select.append($("<option>").val("").text("-- Selecione --"));
    }
};