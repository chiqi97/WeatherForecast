# Introduction 
Weather forecast api is used to handle geographic coordinates and weather forecast for them.
The project is configured to automatically run the migration and create an instance of the SQLite database after it is built.
You can also build the project using dockerfile

# How to run project? By Docker
1. Open cmd and go to your local path where Dockerfile is located. (for example - cd C:\Users\CezaryZysk\RiderProjects\WeatherForecast)
2. Build the docker image with the script: "docker build -t weather-forecast ."
3. Start the docker container with the command: "docker run -p 9084:9084 weather-forecast"
4. If port 9084 on your computer is occupied please change it in DockerFile and then in the docker run command.

# How to run project? BY IDE
1. The project should automatically create a database in the WeatherForecast.Data project when it is built.



