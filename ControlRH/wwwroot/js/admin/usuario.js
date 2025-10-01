window.Usuario = {
    init: function () {

        $(document).ready(function () {

            $('#ColaboradorId').change(function () {
                var cpf = $(this).find(':selected').data('cpf');
                if (cpf) {
                    debugger;
                    var cpfFormatado = window.Usuario.formatarCpf(String(cpf));
                    var primeiros4 = cpfFormatado.substring(0, 5);

                    $('#ColaboradorCpf').val(`${primeiros4}**.***-**`);
                    $('#CpfExibicao').val(`${primeiros4}**.***-**`);
                } else {
                    $('#ColaboradorCpf').val('');
                    $('#CpfExibicao').val('');
                }
            });

        });

    },
    validarCPF: function (cpf) {

        cpf = cpf.replace(/[^\d]+/g, '');
        if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) return false;

        let soma = 0, resto;
        for (let i = 1; i <= 9; i++) {
            soma += parseInt(cpf.substring(i - 1, i)) * (11 - i);
        }
        resto = (soma * 10) % 11;
        if ((resto === 10) || (resto === 11)) resto = 0;
        if (resto !== parseInt(cpf.substring(9, 10))) return false;

        soma = 0;
        for (let i = 1; i <= 10; i++) {
            soma += parseInt(cpf.substring(i - 1, i)) * (12 - i);
        }
        resto = (soma * 10) % 11;
        if ((resto === 10) || (resto === 11)) resto = 0;
        if (resto !== parseInt(cpf.substring(10, 11))) return false;

        return true;
    },
    formatarCpf: function (cpf) {
        cpf = cpf.replace(/\D/g, '');// Retorna como está se não for CPF válido
        return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    }
};