window.Colaborador = {
    init: function () {
        $(document).ready(function () {
            $('#Cpf').mask('000.000.000-00');
            $('#Pis').mask('000.00000.00-0');
            $('#Matricula').mask('00000');

            $('form').on('submit', function () {
                $('#Cpf').val($('#Cpf').cleanVal());
                $('#Pis').val($('#Pis').cleanVal());
                $('#Matricula').val($('#Matricula').cleanVal());
            });
        });
    }
}