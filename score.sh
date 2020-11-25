#!/bin/sh
fuser -k 8001/tcp
count=1
cd FootBallTournament
dotnet run > server.txt 2>&1 &
pid=$!
while [ "$(cat server.txt | grep 'Application started. Press Ctrl+C to shut down.')" == "" ] && [ $count -lt 40 ]
do
sleep 1
count=$((count + 1))
done
cd ..
cd FootBallTournament.Tests
rm -rf reports
dotnet build
dotnet test --filter FullyQualifiedName~FootBallTournament.Tests.FootBallTournamentTests  > output.txt
PASS=$(grep -io 'Passed: [0-9]*' ./output.txt | cut -d' ' -f2)
if [ "$PASS" -eq 7 ]
then dotnet test --filter FullyQualifiedName~FootBallTournament.Tests.NewFeatureTests --logger xunit --results-directory ./reports/
fi