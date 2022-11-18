using lab5;

const int votersCount = 5;

var server = new BlindSignature.Server();
var voters = new List<BlindSignature.Voter>();

var rnd = new Random();

for (var i = 0; i < votersCount; i++)
{
    var voterData = new Tuple<int, byte>(rnd.Next(1000000, 9999999), (byte)rnd.Next(1, 3));
    voters.Add(new BlindSignature.Voter(server.N, server.D, voterData.Item1,voterData.Item2));
    Console.WriteLine("Passport: {0} vote: {1}",voterData.Item1,voterData.Item2);
}

Console.WriteLine();

foreach (var voter in voters)
{
    if (!server.Voters.Contains(voter.H1))
    {
        var bulletin = server.RegisterVoter(voter.H1);
        voter.CreateS(bulletin);
        if (server.CheckBulletin(voter.Message, voter.S))
        {
            Console.WriteLine("Ok");
        }
    }
}

server.CheckBulletin(voters[3].Message, voters[3].S);