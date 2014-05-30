using System.Threading;
using Client.Client;
using Rhino.Mocks;

namespace ClientGUI.Model
{
    public static class ClientFactory
    {
        public enum ClientType { MOCK, LIVE }

        public static IClient CreateClient(ClientType clientType)
        {
            if (clientType == ClientType.MOCK)
            {
                IClient mockedClient = MockRepository.GenerateMock<IClient>();
                mockedClient.Stub(x => x.GetAvailableGameTypes()).Return(new[] {"Chess", "Quatro"});
                mockedClient.Stub(x => x.GetGamesInLobby(Arg<string>.Is.Anything)).Return(new[] {1, 2, 3, 4, 5});
                mockedClient.Stub(x => x.JoinGame(Arg<string>.Is.Anything)).Return(true);
                mockedClient.Stub(x => x.ConnectToServer(Arg<string>.Is.Anything, Arg<string>.Is.Anything))
                    .WhenCalled(x => Thread.Sleep(5000))
                    .Return(true);
                mockedClient.Stub(x => x.GetAvailableAIAlgorithms()).Return(new[] {"MinMax", "AlfaBeta"});
                return mockedClient;
            }
            return new GameClient();
        }

    }
}
