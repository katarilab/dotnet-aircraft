docker build -t dotnet-aircraft .
sudo docker tag dotnet-aircraft:latest f4phantom.skylab:5000/dotnet-aircraft:latest
sudo docker push f4phantom.skylab:5000/dotnet-aircraft:latest
