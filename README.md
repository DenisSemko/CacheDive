<img src="https://github.com/DenisSemko/CacheDive/blob/main/src/Web/client/src/assets/logo.png" alt="logo" title="logo" align="right" height="80" />

# CacheDive
> A Deep Dive into Databases Caching

## Table of Contents
* [General Info](#general-information)
* [Technologies Used](#technologies-used)
* [Architecture](#architecture)
* [Features](#features)
* [Screenshots](#screenshots)
* [Setup](#setup)
* [Usage](#usage)
* [Project Status](#project-status)
* [Contact](#contact)
* [License](#license)

## General Information
The experiment-oriented system helps you choose the appropriate database for caching purposes. An experiment is built on the e-commerce app, as it covers a huge volume of various nuances and requires a detailed system to meet the client's needs.
Master's Diploma.

System provides a friendly UI along with REST API requests to work with the theme.

At the heart of the project lies a powerful stack comprising .NET, React.js, and Microservices Architecture. This robust foundation ensures that the platform is highly scalable, reliable, and provides seamless integration across various components.

## Technologies Used
- Web Application - React.js
- Back-end: .NET 7, C#
- Containerization: Docker
- Databases: MSSQL, MongoDb, Redis
- Microservices Architecture
- API Gateway - Ocelot API
- Identity - ASP.NET Identity, PostgreSQL database
- Microservices Communication - RabbitMQ via MassTransit

## Architecture

<img width="1627" alt="Screenshot 2024-09-30 at 15 44 37" src="https://github.com/user-attachments/assets/c0415af1-6f00-4509-91a2-fca30ed925ad">

## Features
You are able to:
- Configure data for the experiment for all databases using a JSON format file at once.
- Create the experiment with proper configurations.
- See the experiment run result.
- Get the Analytics on the experiments.

## Screenshots

<img width="375" alt="image" src="https://github.com/user-attachments/assets/943056b8-3b6a-414b-8653-cf35bd3334a1">

<img width="377" alt="image" src="https://github.com/user-attachments/assets/dbbb32a3-a884-4b2e-b6ca-b9a84e1e22c5">

<img width="459" alt="image" src="https://github.com/user-attachments/assets/63537386-99a1-4b69-85e9-e74cd9107b29">

<img width="409" alt="image" src="https://github.com/user-attachments/assets/c11989d6-ca99-4583-a3f8-52e91d03a6a1">


## Setup
Project is built locally and it uses SSL certificate to run the Web App securely.

Make sure you have installed and configured [docker](https://docs.docker.com/desktop/install/windows-install/) in your environment. After that, you need to run the below commands from the /src/ directory.

`docker-compose build`
`docker-compose up`

*Another Approach:*

You need to download this repository and run it using Visual Studio 2022, a newer version, or any other IDE that suits you.

You can run the Web application via installing all the dependencies with the command `npm install`.

After that, use the command `HTTPS=true SSL_CRT_FILE={CERT-PATH} SSL_KEY_FILE={KEY-PATH} npm start` for Linux/MacOS systems or `set HTTPS=true&&SSL_CRT_FILE={CERT-PATH}&&SSL_KEY_FILE={KEY-PATH}&&npm start` for Windows!


> You need to make sure you have installed MSSQL, MongoDb, PostgreSQL, Redis & RabbitMQ locally or via Docker.

## Project Status
Finished

## Contact
Created by [@dench327](https://www.linkedin.com/in/denis-semko-551b91191) - feel free to contact me!

© 2024

## License
> You can check out the full license [here](https://github.com/DenisSemko/CacheDive/blob/main/LICENSE).
This project is licensed under the terms of the MIT license.
