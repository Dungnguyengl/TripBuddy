#
# Dockerfile for Eureka Server
#

FROM openjdk:18

MAINTAINER Oreste Luci

# Install netcat
# RUN apt-get update && apt-get install -y netcat

VOLUME /tmp

WORKDIR /eureka-server

ADD target/eureka-service.jar eureka-service.jar

RUN bash -c 'touch /eureka-service.jar'

EXPOSE 8761
CMD ["java", "-jar", "eureka-service.jar"]
