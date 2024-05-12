# Build Docker image
docker build -t dockerimagebotmd .

# Run Docker container in detached mode, mapping ports 41999 to 80 and 41998 to 443
docker run -d -p 41999:8080 -p 41998:443 --name dockerbmd -it dockerimagebotmd