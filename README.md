# 🚀 PersonalHub

PersonalHub est une application de productivité personnelle développée avec **ASP.NET Core 10**, **Blazor Server**, **Clean Architecture** et **OpenAI**.

L'objectif du projet est de centraliser la gestion des notes, objectifs et données personnelles dans une plateforme moderne, évolutive et prête pour des fonctionnalités IA avancées.

---

## ✨ Fonctionnalités

### 🔐 Authentification

* Inscription utilisateur
* Connexion JWT
* Gestion des rôles
* Sécurisation des endpoints API

### 👤 Profil utilisateur

* Consultation du profil
* Modification des informations personnelles
* Gestion des coordonnées utilisateur

### 📝 Notes

* Création de notes
* Modification de notes
* Suppression de notes
* Consultation détaillée
* Gestion personnelle des contenus

### 🎯 Goals (Objectifs)

* Création d'objectifs
* Définition d'une valeur cible
* Suivi de progression
* Date limite optionnelle
* Statut automatique :

  * Active
  * Completed
  * Expired

### 🤖 AI Goal Advisor

Analyse automatique d'un objectif grâce à OpenAI :

* Évaluation de la progression
* Identification des risques
* Conseils personnalisés
* Recommandations d'actions

Exemple :

> Goal: Learn Blazor
> Progress: 40%
> Deadline: 30 days

L'IA génère alors un plan d'action adapté à l'objectif.

### 📊 Dashboard

Statistiques globales :

* Nombre d'utilisateurs
* Nombre de notes
* Nombre d'objectifs
* Répartition des statuts

---

## 🏗 Architecture

Le projet suit les principes de la Clean Architecture :

text
src/
│
├── PersonalHub.Api
├── PersonalHub.Application
├── PersonalHub.Domain
├── PersonalHub.Infrastructure
└── PersonalHub.Web


### Domain

Contient :

* Entités métier
* Enums
* Règles métier

### Application

Contient :

* CQRS
* MediatR
* Commands
* Queries
* Validators
* DTOs
* Interfaces

### Infrastructure

Contient :

* Entity Framework Core
* SQL Server
* ASP.NET Identity
* Services OpenAI
* Services externes

### API

Contient :

* Endpoints Minimal API
* Authentification JWT
* Middlewares

### Web

Contient :

* Blazor Server
* MudBlazor
* Services HTTP
* Pages utilisateur

---

## 🛠 Technologies

### Backend

* ASP.NET Core 10
* Minimal APIs
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* MediatR
* FluentValidation
* Serilog

### Frontend

* Blazor Server
* MudBlazor

### Intelligence Artificielle

* OpenAI API
* GPT-4.1 Mini

---


## 📸 Aperçu

Fonctionnalités principales :

* Dashboard utilisateur
* Gestion des notes
* Gestion des objectifs
* Conseils IA personnalisés
* Authentification sécurisée

---



## 👨‍💻 Auteur

Projet personnel développé pour explorer :

* Clean Architecture
* CQRS
* ASP.NET Core 10
* Blazor
* OpenAI Integration
* Enterprise Development Patterns
