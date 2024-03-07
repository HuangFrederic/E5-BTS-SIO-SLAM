<?php
session_start();
require 'connexion.php'; 
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Votre profile - Bibliothèque Delacroix</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
</head>
<body>

<!-- navbar -->
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
  <a class="navbar-brand" href="menu.php">Bibliothèque</a>
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>

<!-- menu -->
  <div class="collapse navbar-collapse" id="navbarSupportedContent">
    <ul class="navbar-nav mr-auto">
      <li class="nav-item active">
        <a class="nav-link" href="book.php">Liste des livres<span class="sr-only">(current)</span></a>
      </li>

      <li class="nav-item active">
        <a class="nav-link" href="auteur.php">Liste des auteurs<span class="sr-only">(current)</span></a>
      </li>

      <li class="nav-item">
        <a class="nav-link" href="louer.php">Louer un livre</a>
      </li>

      <li class="nav-item">
        <a class="nav-link" href="rendre.php">Rendre un livre</a>
      </li>
    </ul>

<!-- bouton chercher -->
<form class="form-inline my-2 my-lg-0">
      <input class="form-control mr-sm-2" type="search" placeholder="Chercher Livre" aria-label="Search" name="search">
      <button class="btn btn-outline-secondary text-white" type="submit" name="search2"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-search" viewBox="0 0 16 16">
  <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
</svg></button>    

<!-- avatar deroulante -->
<ul class="navbar-nav ms-auto">
<li class="nav-item dropdown">
        <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
        <img
            src="../images/blank.jpg"
            class="rounded-circle"
            height="35"
            width="37"
            alt="Blank Portrait"
            loading="lazy"
          />
        </a>

        <div class="dropdown-menu dropdown-menu-right">
    <div class="text-center font-weight-bold"><?php echo "$_SESSION[prenom] ".$_SESSION["nom"].""; ?> </div>        
    <div class="dropdown-divider"></div>
    <button class="dropdown-item" type="button"><a href="profil.php">Profil</a></button>
    <button class="dropdown-item" type="button"><a href="historique.php">Historique des livres empruntés</a></button>
    <button class="dropdown-item" type="button"><a href="logout.php">Se déconnecter</a></button>
  </div>
      </li>
</ul>

    </form>
  </div>
</nav>

<br><br>

<form method="POST" action="#" id="mdp">
<?php
if (isset($_GET['success']) && $_GET['success'] == 1) {
    echo "<h4><div class='bg-light row justify-content-center'>Modification réussie</div></h4>";
}
?>

<h3>Modifier votre profil</h3>

    <?php //pour remplir les values avec ceux de la bdd
    $tabProfil = getProfil();
    foreach ($tabProfil as $profil): 
    ?>


<div class="col-5">
    <label class="form-label">Nom</label>
    <input type="text" class="form-control" name="nom"  oninput="validateNom(event)" value="<?php echo "".$profil['nom']."" ?>"> 
    <span id="error-message1" style="color: red;"></span><br>
</div>

<div class="col-5">
    <label class="form-label">Prenom</label>
    <input type="text" class="form-control" name="prenom" oninput="validatePrenom(event)" value="<?php echo "".$profil['prenom']."" ?>"> 
    <span id="error-message2" style="color: red;"></span><br>
</div>

<div class="col-5">
    <label class="form-label">Email</label>
    <input type="text" class="form-control" name="email" value="<?php echo "".$profil['email']."" ?>"> <br>
</div>

<div class="col-5">
    <label class="form-label" for="strong">Mot de passe</label>
    <input type="password" class="form-control" id="strong" name="mdp" />
    <span id="mdp-error" style="color: red;"></span> <br>
</div>

    <?php 
    endforeach; 
    ?>
 
    <button type="submit" class="btn btn-info btn-lg mx-3" name="modif">Enregistrer</button>

<script>
//prenom
function validatePrenom(event) {
    const input = event.target;
    const errorMessage = document.getElementById('error-message2');

    const regex = /[^a-zA-ZÀ-ÿ-]/g; // Accepte les lettres, les tirets et les accents
    if (input.value.match(regex)) {
        errorMessage.textContent = 'Veuillez entrer uniquement des lettres.';
        input.value = input.value.replace(regex, '');
    } else {
        errorMessage.textContent = '';
    }
}

//nom
function validateNom(event) {
    const input = event.target;
    const errorMessage = document.getElementById('error-message1');

    const regex = /[^a-zA-ZÀ-ÿ-]/g; // Accepte les lettres, les tirets et les accents
    if (input.value.match(regex)) {
        errorMessage.textContent = 'Veuillez entrer uniquement des lettres.';
        input.value = input.value.replace(regex, '');
    } else {
        errorMessage.textContent = '';
    }
}
    //pour le mdp
   document.addEventListener('DOMContentLoaded', function() {
    var form = document.getElementById('mdp'); // celui du formulaire
    var mdpInput = document.getElementById('strong'); // celui du mdp
    var mdpError = document.getElementById('mdp-error'); // affiche erreur si incorrecte

    form.addEventListener('submit', function(event) {
        var mdp = mdpInput.value;
        var regex = /^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[A-Z]).{4,}$/;

        if (!regex.test(mdp)) {
            mdpError.textContent = 'Le mot de passe doit avoir au moins 12 caractères, au moins 1 caractère spécial, 1 majuscule et 1 chiffre.';
            event.preventDefault(); // can't submit
        } else {
            mdpError.textContent = ''; // reset si tout est bon
        }
    });
});
</script>

</form>
<div class="container">
  <footer class="d-flex flex-wrap justify-content-between align-items-center py-3 my-4 border-top">
    <div class="col-md-4 d-flex align-items-center">
      <span class="mb-3 mb-md-0 text-body-secondary">© 2023 Biblio, Inc</span>
    </div>
  </footer>
</div>
</body>
</html>

