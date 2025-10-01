window.PontoEletronico = {

    elements: {
        registroModal: document.getElementById('registroModal'),
        registroData: document.getElementById('registroData'),
        registroHora: document.getElementById('registroHora'),
        horaRegistroHidden: document.getElementById('horaRegistroHidden'),
        ultimaSync: document.getElementById('ultimaSync'),
        dataAtual: document.getElementById('dataAtual'),
        horaAtual: document.getElementById('horaAtual'),
        diaSemana: document.getElementById('diaSemana'),
        infoRegistro: document.getElementById('infoRegistro'),
        countdownTimer: document.getElementById('countdownTimer'),
        countdownBox: document.getElementById('countdownBox'),
        tipoMarcacaoSelect: document.querySelector('select[name="TipoMarcacao"]')
    },

    init: function () {
        this.atualizarRelogioTelaPrincipal();
        setInterval(() => this.atualizarRelogioTelaPrincipal(), 1000);

        if (this.elements.registroModal) {
            this.elements.registroModal.addEventListener('show.bs.modal', (event) => {
                this.prepararModalMarcacao();
            });
        }
    },

    atualizarRelogioTelaPrincipal: function () {
        const agora = new Date();
        const optionsHora = {
            timeZone: 'America/Sao_Paulo',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        };

        const horaBR = agora.toLocaleTimeString('pt-BR', optionsHora);

        let dataExtensa = new Intl.DateTimeFormat('pt-BR', {
            timeZone: 'America/Sao_Paulo',
            day: '2-digit',
            month: 'long',
            year: 'numeric'
        }).format(agora);

        dataExtensa = dataExtensa.replace(/(\sde\s)([a-z])/, (match, p1, p2) => p1 + p2.toUpperCase());

        const diaSemana = new Intl.DateTimeFormat('pt-BR', {
            timeZone: 'America/Sao_Paulo',
            weekday: 'long'
        }).format(agora);

        const diaSemanaCapitalizado = this.capitalizeFirst(diaSemana);
        const infoRegistro = `Hoje ${horaBR} GMT-03:00 via Web`;

        if (this.elements.dataAtual) this.elements.dataAtual.textContent = dataExtensa;
        if (this.elements.horaAtual) this.elements.horaAtual.textContent = horaBR.slice(0, 5); // Apenas HH:mm
        if (this.elements.diaSemana) this.elements.diaSemana.textContent = diaSemanaCapitalizado;
        if (this.elements.infoRegistro) this.elements.infoRegistro.textContent = infoRegistro;
    },

    prepararModalMarcacao: function () {
        const agoraModal = new Date(); // Obtém a data e hora exata no momento da abertura do modal

        // Formatação para exibição no modal
        const diaSemanaModal = agoraModal.toLocaleDateString('pt-BR', { weekday: 'long' });
        const dataModal = agoraModal.toLocaleDateString('pt-BR', { day: '2-digit', month: 'long', year: 'numeric' });
        const horaDisplayModal = agoraModal.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
        const horaCompletaExibicaoSync = agoraModal.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit', second: '2-digit' });

        // Formato ISO 8601 (YYYY-MM-DDTHH:mm:ss) para o input hidden, que é o ideal para DateTime no C#
        // Se o C# esperar um formato diferente, ajuste aqui.
        // String(agoraModal.getMonth() + 1).padStart(2, '0') + "-" + String(agoraModal.getDate()).padStart(2, '0') + " " + String(agoraModal.getHours()).padStart(2, '0') + ":" + String(agoraModal.getMinutes()).padStart(2, '0') + ":" + String(agoraModal.getSeconds()).padStart(2, '0');
        // Usaremos uma abordagem mais segura para obter o formato ISO
        // const dataParaEnvio = agoraModal.toISOString().slice(0, 19).replace('T', ' ');

        // Função auxiliar para garantir dois dígitos (ex: 01, 05)
        const pad = (num) => num < 10 ? '0' + num : num;

        // --- Para Envio ao C# (Formato YYYY-MM-DD HH:MM:SS com hora local) ---
        const ano = agoraModal.getFullYear();
        const mes = pad(agoraModal.getMonth() + 1);    // getMonth() é base 0
        const dia = pad(agoraModal.getDate());
        const horas = pad(agoraModal.getHours());      // Hora no fuso horário local
        const minutos = pad(agoraModal.getMinutes());  // Minutos no fuso horário local
        const segundos = pad(agoraModal.getSeconds()); // Segundos no fuso horário local

        const dataParaEnvio = `${ano}-${mes}-${dia} ${horas}:${minutos}:${segundos}`;

        // Preenche os elementos do modal
        if (this.elements.registroData) this.elements.registroData.textContent = `${this.capitalizeFirst(diaSemanaModal)}, ${dataModal}`;
        if (this.elements.registroHora) this.elements.registroHora.textContent = horaDisplayModal;

        // **AQUI ESTÁ O FOCO DA ATUALIZAÇÃO PARA O INPUT HIDDEN**
        if (this.elements.horaRegistroHidden) {
            this.elements.horaRegistroHidden.value = dataParaEnvio;
        }

        // Atualiza a 'Última sincronização' no topo do card principal
        if (this.elements.ultimaSync) {
            this.elements.ultimaSync.textContent = `Última sincronização: ${horaCompletaExibicaoSync}`;
        }
    },

    capitalizeFirst: function (str) {
        if (!str) return '';
        return str.charAt(0).toUpperCase() + str.slice(1);
    },

    // Funções de countdown (mantidas como estão)
    startCountdown: function (durationInMinutes) {
        const countdownTimer = this.elements.countdownTimer;
        const countdownBox = this.elements.countdownBox;

        if (!countdownTimer || !countdownBox) return;

        let totalSeconds = durationInMinutes * 60;
        clearInterval(window.PontoEletronico._countdownInterval);

        window.PontoEletronico._countdownInterval = setInterval(() => {
            const minutes = Math.floor(totalSeconds / 60);
            const seconds = totalSeconds % 60;

            countdownTimer.textContent = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;

            if (totalSeconds <= 60) {
                countdownBox.classList.remove('pulse-red');
                countdownBox.classList.add('pulse-yellow');
            } else {
                countdownBox.classList.remove('pulse-red', 'pulse-yellow');
            }

            if (totalSeconds <= 0) {
                clearInterval(window.PontoEletronico._countdownInterval);
                countdownBox.classList.remove('pulse-red', 'pulse-yellow');
                countdownTimer.textContent = "00:00";
            } else {
                totalSeconds--;
            }
        }, 1000);
    },

    _countdownInterval: null
};