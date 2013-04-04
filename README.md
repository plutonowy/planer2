planer2
=======

PLANER_2

// znajdz plik: .git/info/exclude
// i na samym koncu dodaj linijke "*.userprefs" oczywiscie bez cudzyslowia
oraz linijke "*.sln" //

następnie wklepujesz:
"git rm -R ."
potem "git add ."

i sprawdzas: "git status --ignored" czy pliki o tych rozszerzeniach beda ignorowane i na koniec dajesz

git commit -m "dodanie .sln i .userprefs do listy ignorowanych"
git push -u

i powinno działać

// jesli masz już takie coś to nie zmieniaj nic ;) chodzi o to by wykluczyć od sledzenia te pliki
// one i tak co chwila sie zmieniaja
