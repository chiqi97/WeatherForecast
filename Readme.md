# Introduction 
The Weather Forecast API is designed to manage geographic coordinates and provide weather forecasts for them. 
The project is configured to automatically run database migrations and create an instance of the SQLite database upon build. You can also build and run the project using Docker.

# How to run project? By Docker
1. Open a command prompt and navigate to the directory where the Dockerfile is located. (for example - cd C:\Users\RiderProjects\WeatherForecast)
2. Build the Docker image using the following command: "docker build -t weather-forecast ."
3. Start the Docker container with this command: "docker run -p 9084:9084 weather-forecast"
4. If port 9084 on your machine is occupied, you can change the port in the Dockerfile, and update the port in the docker run command accordingly.

# How to run project? BY IDE
1.  The project is configured to automatically create a SQLite database in the WeatherForecast.Data project when it is built in your IDE.
    Simply build and run the project, and the necessary database will be generated automatically.



