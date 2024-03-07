<?php ob_start(); ?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>S'inscrire à la bibliothèque - Bibliothèque Delacroix</title>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://unpkg.com/bs-brain@2.0.0/components/logins/login-8/assets/css/login-8.css" />
</head>
<body>

<?php
require_once("connexion.php");
?>

<form method="POST" id="mdp">

<section class="vh-100" style="background-color: #eee;">
  <div class="container h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
      <div class="col-lg-12 col-xl-11">
        <div class="card text-black" style="border-radius: 25px;">
          <div class="card-body p-md-5">
            <div class="row justify-content-center">
              <div class="col-md-10 col-lg-6 col-xl-5 order-2 order-lg-1">

                <p class="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Inscription</p>

                <form class="mx-1 mx-md-4">

                  <div class="d-flex flex-row align-items-center mb-4">
                    <i class="fas fa-user fa-lg me-3 fa-fw"></i>
                    <div class="form-outline flex-fill mb-0">
                      <label class="form-label" for="form3Example1c" >Nom :</label>
                      <input type="text" id="form3Example1c" class="form-control" name="nom" oninput="validateNom(event)"/>
                      <span id="error-message1" style="color: red;"></span> 
                    </div>                 
                  </div>

                  <div class="d-flex flex-row align-items-center mb-4">
                    <i class="fas fa-user fa-lg me-3 fa-fw"></i>
                    <div class="form-outline flex-fill mb-0">
                    <label class="form-label" for="form3Example1c">Prenom :</label>
                    <input type="text" id="form3Example1c" class="form-control" name="prenom" oninput="validatePrenom(event)">
                    <span id="error-message2" style="color: red;"></span>
                    </div>
                  </div>

                  <script>
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
</script>
                
                  <div class="d-flex flex-row align-items-center mb-4">
                    <i class="fas fa-envelope fa-lg me-3 fa-fw"></i>
                    <div class="form-outline flex-fill mb-0">
                      <label class="form-label" for="form3Example3c" >Email :</label>
                      <input type="email" id="form3Example3c" class="form-control" name="email"/>                    
                    </div>
                  </div>

                  <div class="d-flex flex-column mb-4">
    <div class="d-flex flex-row align-items-center">
        <i class="fas fa-lock fa-lg me-3 fa-fw"></i>
        <div class="form-outline flex-fill mb-0">
            <label class="form-label" for="strong">Mot de passe :</label>
            <input type="password" id="strong" class="form-control" name="mdp"/>
        </div>
    </div>
    <div class="d-flex justify-content-start">
        <span id="mdp-error" style="color: red;"></span>
    </div>
</div>

<script>
   document.addEventListener('DOMContentLoaded', function() {
    var form = document.getElementById('mdp'); // celui du formulaire
    var mdpInput = document.getElementById('strong'); // celui du mdp
    var mdpError = document.getElementById('mdp-error'); // affiche erreur si incorrecte

    form.addEventListener('submit', function(event) {
        var mdp = mdpInput.value;
        var regex = /^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[A-Z]).{4,}$/; /*regex: regle precis */

        if (!regex.test(mdp)) { /*test: verifie si respecte la regle */
            mdpError.textContent = 'Le mot de passe doit avoir au moins 12 caractères, au moins 1 caractère spécial, 1 majuscule et 1 chiffre.';
            event.preventDefault(); // can't submit
        } else {
            mdpError.textContent = ''; // reset si tout est bon
        }
    });
});
</script>
                  <div class="d-flex justify-content-center mx-4 mb-3 mb-lg-4">
                    <button type="submit" class="btn btn-primary btn-lg" name="register">S'inscrire</button>
                  </div>

                </form>

              </div>
              <div class="col-md-10 col-lg-6 col-xl-7 d-flex align-items-center order-1 order-lg-2">

                <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp"
                  class="img-fluid" alt="Sample image">

              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
 </div>
</section>
<?php
if(isset($_POST['register'])){   
    array_pop($_POST);

    $hashedPassword = password_hash($_POST['mdp'], PASSWORD_DEFAULT);
    $_POST['mdp'] = $hashedPassword;

    $rep = register($_POST);

    if($rep){
        header('location:index.php?success=1');
        exit();
    }
    else{
        echo "<script>Erreur, veuillez réessayer</script>";
    }
}
ob_end_flush();
?>
</form>
</body>
</html>