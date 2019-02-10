# Get MongoDB
FROM mongo:latest

# Install any needed packages
RUN apt-get update
RUN apt-get install wget
RUN apt-get install unzip

# Set the working directory to /app
WORKDIR /src

# Download the data
RUN wget http://cs.stanford.edu/people/alecmgo/trainingandtestdata.zip

# Unzip the data
RUN unzip trainingandtestdata.zip

# Add header row
RUN sed -i '1s;^;polarity,id,date,query,user,text\n;' training.1600000.processed.noemoticon.csv

# set the hostname of the dbserver to the name given in the docker-compose file
ENV SERVER="10.0.75.2"
ENV PORT="27017"

# when done run: docker exec -d assignment_2_db_mongodb_1 mongoimport --drop --db social_net --collection tweets --type csv --headerline --file training.1600000.processed.noemoticon.csv