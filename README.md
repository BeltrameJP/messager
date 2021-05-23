# messager
Messager feito em C#. Utiliza do terrminal para enviar e receber mensagens. 
O projeto abriga 3 sub projetos: Client, server, e tests.
 - O Projeto servidor deve ser rodado naturalmente com um dotnet run
 - O Cliente deve ser executado da mesma forma, e o IP de conexão será inserido manualmente.
 - Os testes devem ser rodados com o comando: dotnet test

Features criadas:
 - Envio de mensagem privada
 - Envio de mensagem em broadcasts
 - Criação de salas
 - Mudança de salas
 - Envio de mensagens para todos na sala

Casos que os testes unitários cobrem:
 - Enviar mensagem privada
 - Verificação de nicknames iguais
 - Mensagens em broadcast
