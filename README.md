# 🎓 AuditoriumHub

### System zarządzania rezerwacjami sal dydaktycznych

**AuditoriumHub** to aplikacja napisana w języku **C#**, której celem jest usprawnienie zarządzania salami dydaktycznymi, ich wyposażeniem oraz rezerwacjami w środowisku akademickim.

Projekt powstał w ramach przedmiotu **Programowanie Obiektowe (OOP)** i koncentruje się na praktycznym wykorzystaniu zasad programowania obiektowego, architektury warstwowej oraz wzorców projektowych.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Programming-239120?logo=sharp&logoColor=white)
![Status](https://img.shields.io/badge/Status-In%20Development-orange)
![Tests](https://img.shields.io/badge/Integration%20Tests-35%20Passed-brightgreen)

---

## 📋 O projekcie

System umożliwia zarządzanie infrastrukturą dydaktyczną uczelni, w szczególności budynkami, salami, użytkownikami oraz rezerwacjami.

Głównym założeniem projektu jest oddzielenie logiki biznesowej od warstwy dostępu do danych, co pozwala na dalszą rozbudowę aplikacji oraz zmianę sposobu przechowywania informacji bez konieczności przebudowy całego systemu.

## ✨ Funkcjonalności

### 🏢 Zarządzanie budynkami i salami

- Dodawanie, edytowanie i usuwanie budynków.
- Zarządzanie salami dydaktycznymi.
- Określanie pojemności i dostępności sal.
- Wyszukiwanie wolnych sal w wybranym przedziale czasowym.
- Ochrona przed usuwaniem budynków posiadających sale.
- Blokowanie usuwania sal z aktywnymi rezerwacjami.

### 📅 System rezerwacji

- Tworzenie nowych rezerwacji.
- Edytowanie terminów i danych rezerwacji.
- Zmiana przypisanej sali.
- Anulowanie istniejących rezerwacji.
- Automatyczne wykrywanie konfliktów czasowych.
- Weryfikacja dostępności sal.
- Ponowne udostępnianie sal po anulowaniu rezerwacji.

### 👥 Zarządzanie użytkownikami

- Obsługa administratorów i prowadzących.
- Dodawanie i edycja użytkowników.
- Weryfikacja unikalności adresów e-mail.
- Wyszukiwanie użytkowników.
- Dezaktywacja kont.
- Ochrona danych użytkowników powiązanych z rezerwacjami.

### 🖥️ Zarządzanie wyposażeniem

- Dodawanie i edytowanie wyposażenia.
- Przypisywanie wyposażenia do sal.
- Określanie liczby dostępnych urządzeń.
- Usuwanie przypisań wyposażenia.
- Kontrola powiązań między wyposażeniem a salami.

### 🗓️ Harmonogram rezerwacji

- Wyświetlanie harmonogramu dziennego.
- Wyświetlanie harmonogramu tygodniowego.
- Przegląd rezerwacji wybranej sali.
- Przegląd rezerwacji konkretnego użytkownika.
- Chronologiczne sortowanie rezerwacji.

---

## 🛠️ Technologie

| Technologia | Zastosowanie |
|---|---|
| C# | Język programowania |
| .NET 10 | Platforma aplikacji |
| LINQ | Filtrowanie i przetwarzanie danych |
| Visual Studio | Środowisko programistyczne |
| Git & GitHub | Kontrola wersji |
| In-Memory Storage | Przechowywanie danych podczas działania aplikacji |

## 🏗️ Architektura projektu

Projekt wykorzystuje architekturę warstwową, w której poszczególne komponenty odpowiadają za odrębne zadania.

| Projekt | Odpowiedzialność |
|---|---|
| **AuditoriumHub** | Aplikacja konsolowa i testy integracyjne |
| **AuditoriumHub.DataModel** | Encje, relacje i typy wyliczeniowe |
| **AuditoriumHub.ServiceAbstractions** | Interfejsy usług i repozytoriów |
| **AuditoriumHub.DataAccess.Memory** | Implementacja dostępu do danych w pamięci |
| **AuditoriumHub.Services.Memory** | Implementacja logiki biznesowej |

### Zastosowane wzorce i zasady

- **Repository Pattern** — ujednolicony interfejs dostępu do danych.
- **Unit of Work Pattern** — wspólny punkt dostępu do repozytoriów i operacji zapisu.
- **Dependency Injection** — przekazywanie zależności przez konstruktory.
- **Separation of Concerns** — rozdzielenie odpowiedzialności między warstwami.
- **Abstraction & Polymorphism** — wykorzystanie interfejsów i dziedziczenia.
- **Encapsulation** — umieszczenie wybranych operacji biznesowych bezpośrednio w encjach.

## 📦 Model danych

Główne encje systemu:

- `Budynek` — budynek uczelni.
- `Sala` — sala dydaktyczna.
- `Uzytkownik` — abstrakcyjna klasa bazowa użytkownika.
- `Administrator` — użytkownik administrujący systemem.
- `Prowadzacy` — prowadzący zajęcia.
- `Rezerwacja` — rezerwacja sali w określonym terminie.
- `HistoriaRezerwacji` — model historii operacji na rezerwacjach.
- `Wyposazenie` — rodzaj wyposażenia.
- `WyposazenieSali` — powiązanie wyposażenia z salą wraz z jego ilością.

## 🧪 Testowanie

Działanie systemu zostało sprawdzone za pomocą testów integracyjnych uruchamianych w aplikacji konsolowej.

**Wyniki ostatniego uruchomienia:**

| Wynik | Liczba |
|---|---:|
| ✅ Testy zaliczone | 35 |
| ❌ Testy niezaliczone | 0 |
| Łącznie | 35 |

Przetestowane zostały między innymi operacje CRUD, walidacja danych, wykrywanie konfliktów rezerwacji, obsługa anulowania, generowanie harmonogramów oraz ochrona powiązanych danych.

Testy obejmują wybrane scenariusze działania systemu i nie zastępują pełnego zestawu testów jednostkowych.

---

## 🚀 Uruchomienie projektu

### Wymagania

- .NET SDK 10.0
- Visual Studio z obsługą .NET lub inne kompatybilne środowisko programistyczne
- Git

### Instalacja

**1. Sklonuj repozytorium:**

`git clone https://github.com/aernjdz/AuditoriumHub.git`

**2. Przejdź do katalogu projektu:**

`cd AuditoriumHub`

**3. Otwórz rozwiązanie w Visual Studio.**

**4. Skompiluj projekt:**

`dotnet build`

**5. Uruchom aplikację:**

`dotnet run --project AuditoriumHub`

### Ważne

Aktualna implementacja wykorzystuje przechowywanie danych w pamięci operacyjnej. Dane nie są trwale zapisywane i zostają utracone po zakończeniu działania aplikacji.

---

## 🔮 Plany rozwoju

W kolejnych etapach planowane są:

- [ ] Pełna implementacja historii rezerwacji.
- [ ] Rozszerzenie walidacji i reguł biznesowych.
- [ ] Automatyczne generowanie identyfikatorów encji.
- [ ] Rozbudowa filtrowania sal według wyposażenia i budynku.
- [ ] Implementacja trwałego prze
