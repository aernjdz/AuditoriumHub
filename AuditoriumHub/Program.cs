
using AuditoriumHub.DataAccess.Memory.Context;
using AuditoriumHub.DataAccess.Memory.UnitOfWork;
using AuditoriumHub.DataModel.Entities;
using AuditoriumHub.DataModel.Entities.Users;
using AuditoriumHub.DataModel.Enums;
using AuditoriumHub.Services.Memory.Services;

var context = new MemoryDbContext();
var unitOfWork = new UnitOfWorkMemory(context);

var budynekService = new BudynekService(unitOfWork);
var salaService = new SalaService(unitOfWork);
var uzytkownikService = new UzytkownikService(unitOfWork);
var rezerwacjaService = new RezerwacjaService(unitOfWork);
var harmonogramService = new HarmonogramService(unitOfWork);
var wyposazenieService = new WyposazenieService(unitOfWork);

int zaliczone = 0;
int niezaliczone = 0;

void Test(string nazwa, Action akcja)
{
    try
    {
        akcja();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[PASS] {nazwa}");
        zaliczone++;
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[FAIL] {nazwa}: {ex.Message}");
        niezaliczone++;
    }
    finally
    {
        Console.ResetColor();
    }
}

void Sprawdz(bool warunek, string komunikat)
{
    if (!warunek)
        throw new Exception(komunikat);
}

void OczekujWyjatku<T>(Action akcja)
    where T : Exception
{
    try
    {
        akcja();
    }
    catch (T)
    {
        return;
    }

    throw new Exception(
        $"Oczekiwano wyjątku {typeof(T).Name}.");
}

void Sekcja(string nazwa)
{
    Console.WriteLine();
    Console.WriteLine($"========== {nazwa} ==========");
}

Console.WriteLine("AUDITORIUM HUB - TESTY INTEGRACYJNE");

// =======================================
// 1. BUDYNKI
// =======================================

Sekcja("BUDYNEK SERVICE");

var budynek = new Budynek
{
    Id = 1,
    Nazwa = "Budynek A",
    Adres = "ul. Akademicka 1"
};

Test("Dodawanie budynku", () =>
{
    budynekService.DodajBudynek(budynek);

    Sprawdz(
        budynekService.PobierzWszystkieBudynki().Count == 1,
        "Nie dodano budynku.");
});

Test("Pobieranie budynku", () =>
{
    var wynik = budynekService.PobierzBudynek(1);

    Sprawdz(wynik != null, "Nie znaleziono budynku.");
    Sprawdz(wynik?.Nazwa == "Budynek A", "Błędna nazwa.");
});

Test("Edycja budynku", () =>
{
    budynekService.EdytujBudynek(new Budynek
    {
        Id = 1,
        Nazwa = "Budynek Informatyki",
        Adres = "ul. Akademicka 1"
    });

    Sprawdz(
        budynekService.PobierzBudynek(1)?.Nazwa
            == "Budynek Informatyki",
        "Nie zmieniono nazwy.");
});

Test("Odrzucenie pustej nazwy budynku", () =>
{
    OczekujWyjatku<ArgumentException>(() =>
        budynekService.DodajBudynek(new Budynek
        {
            Id = 2,
            Nazwa = "",
            Adres = "Test"
        }));
});

// =======================================
// 2. SALE
// =======================================

Sekcja("SALA SERVICE");

var sala = new Sala
{
    Id = 1,
    Numer = "101",
    Pojemnosc = 30,
    BudynekId = 1,
    CzyAktywna = true
};

Test("Dodawanie sali", () =>
{
    salaService.DodajSale(sala);

    Sprawdz(
        salaService.PobierzWszystkieSale().Count == 1,
        "Nie dodano sali.");
});

Test("Odrzucenie sali bez budynku", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        salaService.DodajSale(new Sala
        {
            Id = 2,
            Numer = "102",
            Pojemnosc = 20,
            BudynekId = 999
        }));
});

Test("Odrzucenie nieprawidłowej pojemności", () =>
{
    OczekujWyjatku<ArgumentException>(() =>
        salaService.DodajSale(new Sala
        {
            Id = 2,
            Numer = "102",
            Pojemnosc = -5,
            BudynekId = 1
        }));
});

Test("Edycja sali", () =>
{
    salaService.EdytujSale(new Sala
    {
        Id = 1,
        Numer = "101",
        Pojemnosc = 40,
        BudynekId = 1
    });

    Sprawdz(
        salaService.PobierzSale(1)?.Pojemnosc == 40,
        "Nie zmieniono pojemności.");
});

Test("Pobieranie sal budynku", () =>
{
    var sale = budynekService.PobierzSaleBudynku(1);

    Sprawdz(sale.Count == 1, "Nie znaleziono sali.");
});

Test("Blokowanie usuwania budynku z salami", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        budynekService.UsunBudynek(1));
});

// =======================================
// 3. UZYTKOWNICY
// =======================================

Sekcja("UZYTKOWNIK SERVICE");

var prowadzacy = new Prowadzacy
{
    Id = 1,
    Imie = "Jan",
    Nazwisko = "Kowalski",
    Email = "jan@example.com",
    CzyAktywny = true
};

Test("Dodawanie użytkownika", () =>
{
    uzytkownikService.DodajUzytkownika(prowadzacy);

    Sprawdz(
        uzytkownikService.PobierzWszystkichUzytkownikow()
            .Count == 1,
        "Nie dodano użytkownika.");
});

Test("Odrzucenie zduplikowanego emaila", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        uzytkownikService.DodajUzytkownika(new Prowadzacy
        {
            Id = 2,
            Imie = "Adam",
            Nazwisko = "Nowak",
            Email = "JAN@example.com"
        }));
});

Test("Wyszukiwanie po nazwisku", () =>
{
    var wynik = uzytkownikService.ZnajdzPoNazwisku("kow");

    Sprawdz(wynik.Count == 1, "Nie znaleziono użytkownika.");
});

// =======================================
// 4. WYPOSAZENIE
// =======================================

Sekcja("WYPOSAZENIE SERVICE");

var projektor = new Wyposazenie
{
    Id = 1,
    Nazwa = "Projektor",
    Opis = "Projektor multimedialny"
};

Test("Dodawanie wyposażenia", () =>
{
    wyposazenieService.DodajWyposazenie(projektor);

    Sprawdz(
        wyposazenieService.PobierzWszystkieWyposazenia()
            .Count == 1,
        "Nie dodano wyposażenia.");
});

Test("Przypisanie wyposażenia do sali", () =>
{
    wyposazenieService.DodajWyposazenieDoSali(1, 1, 2);

    var wynik = wyposazenieService.PobierzWyposazenieSali(1);

    Sprawdz(
        wynik.Count == 1 && wynik[0].Ilosc == 2,
        "Nie przypisano wyposażenia.");
});

Test("Odrzucenie ponownego przypisania", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        wyposazenieService.DodajWyposazenieDoSali(1, 1, 1));
});

Test("Blokowanie usuwania przypisanego wyposażenia", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        wyposazenieService.UsunWyposazenie(1));
});

// =======================================
// 5. REZERWACJE
// =======================================

Sekcja("REZERWACJA SERVICE");

DateTime data = new DateTime(2026, 11, 20);

var rezerwacja1 = new Rezerwacja
{
    Id = 1,
    SalaId = 1,
    UzytkownikId = 1,
    DataOd = data.AddHours(10),
    DataDo = data.AddHours(12),
    Cel = "Wykład z programowania"
};

Test("Tworzenie rezerwacji", () =>
{
    rezerwacjaService.UtworzRezerwacje(rezerwacja1);

    Sprawdz(
        rezerwacjaService.PobierzRezerwacje(1)?.Status
            == StatusRezerwacji.Aktywna,
        "Rezerwacja nie jest aktywna.");
});

var rezerwacja2 = new Rezerwacja
{
    Id = 2,
    SalaId = 1,
    UzytkownikId = 1,
    DataOd = data.AddHours(11),
    DataDo = data.AddHours(13),
    Cel = "Ćwiczenia"
};

Test("Wykrywanie konfliktu rezerwacji", () =>
{
    Sprawdz(
        rezerwacjaService.SprawdzKonflikt(
            1, data.AddHours(11), data.AddHours(13)),
        "Nie wykryto konfliktu.");
});

Test("Odrzucenie kolidującej rezerwacji", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        rezerwacjaService.UtworzRezerwacje(rezerwacja2));

    Sprawdz(
        rezerwacjaService.PobierzRezerwacje(2) == null,
        "Kolidująca rezerwacja została zapisana.");
});

Test("Odrzucenie niepoprawnego terminu", () =>
{
    OczekujWyjatku<ArgumentException>(() =>
        rezerwacjaService.SprawdzKonflikt(
            1, data.AddHours(15), data.AddHours(10)));
});

Test("Wyszukiwanie wolnych sal", () =>
{
    var wynik = salaService.ZnajdzWolneSale(
        data.AddHours(11), data.AddHours(12), 20);

    Sprawdz(wynik.Count == 0, "Zajęta sala jest dostępna.");
});

Test("Edycja terminu własnej rezerwacji", () =>
{
    rezerwacjaService.EdytujRezerwacje(new Rezerwacja
    {
        Id = 1,
        SalaId = 1,
        UzytkownikId = 1,
        DataOd = data.AddHours(9),
        DataDo = data.AddHours(11),
        Cel = "Nowy termin wykładu"
    });

    var wynik = rezerwacjaService.PobierzRezerwacje(1);

    Sprawdz(
        wynik?.DataOd == data.AddHours(9),
        "Nie zmieniono terminu.");

    Sprawdz(
        wynik?.Cel == "Nowy termin wykładu",
        "Nie zmieniono celu.");
});

Test("Anulowanie rezerwacji", () =>
{
    rezerwacjaService.AnulujRezerwacje(1);

    Sprawdz(
        rezerwacjaService.PobierzRezerwacje(1)?.Status
            == StatusRezerwacji.Anulowana,
        "Nie anulowano rezerwacji.");
});

Test("Blokowanie ponownego anulowania", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        rezerwacjaService.AnulujRezerwacje(1));
});

Test("Ponowne wykorzystanie sali po anulowaniu", () =>
{
    rezerwacjaService.UtworzRezerwacje(rezerwacja2);

    Sprawdz(
        rezerwacjaService.PobierzRezerwacje(2)?.Status
            == StatusRezerwacji.Aktywna,
        "Nie udało się utworzyć rezerwacji.");
});

// =======================================
// 6. HARMONOGRAM
// =======================================

Sekcja("HARMONOGRAM SERVICE");

Test("Harmonogram dzienny", () =>
{
    var wynik = harmonogramService.PobierzDzienny(data);

    Sprawdz(wynik.Count == 1, "Błędny harmonogram dzienny.");
    Sprawdz(wynik[0].Id == 2, "Niepoprawna rezerwacja.");
});

Test("Harmonogram tygodniowy", () =>
{
    var wynik = harmonogramService.PobierzTygodniowy(data);

    Sprawdz(wynik.Count == 1, "Błędny harmonogram tygodniowy.");
});

Test("Harmonogram sali", () =>
{
    var wynik = harmonogramService.PobierzDlaSali(1);

    Sprawdz(
        wynik.Count == 1 && wynik[0].Id == 2,
        "Niepoprawny harmonogram sali.");
});

Test("Harmonogram użytkownika", () =>
{
    var wynik = harmonogramService.PobierzDlaUzytkownika(1);

    Sprawdz(
        wynik.Count == 1 && wynik[0].Id == 2,
        "Niepoprawny harmonogram użytkownika.");
});

// =======================================
// 7. OCHRONA DANYCH
// =======================================

Sekcja("OCHRONA DANYCH");

Test("Blokowanie usunięcia sali z rezerwacją", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        salaService.UsunSale(1));
});

Test("Blokowanie usunięcia użytkownika z rezerwacjami", () =>
{
    OczekujWyjatku<InvalidOperationException>(() =>
        uzytkownikService.UsunUzytkownika(1));
});

Test("Dezaktywowanie użytkownika", () =>
{
    uzytkownikService.DezaktywujUzytkownika(1);

    Sprawdz(
        uzytkownikService.PobierzUzytkownika(1)?
            .CzyAktywny == false,
        "Użytkownik nadal jest aktywny.");
});

Test("Usuwanie wyposażenia z sali", () =>
{
    wyposazenieService.UsunWyposazenieZSali(1, 1);

    Sprawdz(
        wyposazenieService.PobierzWyposazenieSali(1)
            .Count == 0,
        "Wyposażenie nadal jest przypisane.");
});

Test("Usuwanie nieprzypisanego wyposażenia", () =>
{
    wyposazenieService.UsunWyposazenie(1);

    Sprawdz(
        wyposazenieService.PobierzWyposazenie(1) == null,
        "Wyposażenie nie zostało usunięte.");
});

// =======================================
// PODSUMOWANIE
// =======================================

Sekcja("PODSUMOWANIE");

Console.WriteLine($"Zaliczone: {zaliczone}");
Console.WriteLine($"Niezaliczone: {niezaliczone}");
Console.WriteLine($"Łącznie: {zaliczone + niezaliczone}");

if (niezaliczone == 0)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("WSZYSTKIE TESTY ZALICZONE!");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("NIEKTÓRE TESTY NIE POWIODŁY SIĘ!");
}

Console.ResetColor();
