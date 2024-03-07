<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bibliothèque Delacroix - La 2e meilleur source d'emprunt de livre</title>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://unpkg.com/bs-brain@2.0.0/components/logins/login-8/assets/css/login-8.css" />
</head>
<body>
    
<?php
require_once("connexion.php");
?>

<?php
if (isset($_GET['success']) && $_GET['success'] == 1) {
    echo "<h4><div class='bg-light row justify-content-center'>Inscription réussie</div></h4>";
}
?>

<form method="POST" action="flog2.php">

<section class="bg-light p-3 p-md-4 p-xl-5">
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-12 col-xxl-11">
          <div class="card border-light-subtle shadow-sm">
            <div class="row g-0">
              <div class="col-12 col-md-6">
                <img class="img-fluid rounded-start w-100 h-100 object-fit-cover" loading="lazy" src="../images/biblio.jpg" alt="">
              </div>
              <div class="col-12 col-md-6 d-flex align-items-center justify-content-center">
                <div class="col-12 col-lg-11 col-xl-10">
                  <div class="card-body p-3 p-md-4 p-xl-5">
                    <div class="row">
                      <div class="col-12">
                        <div class="mb-5">
                          <div class="text-center mb-4">
                            <a href="#!">
                              <img src="../images/livre1.jpg" alt="" width="175" height="57">
                            </a>
                          </div>
                          <h4 class="text-center">Bibliothèque Delacroix</h4>
                        </div>
                      </div>
                    </div>

                    <form action="#!">
                      <div class="row gy-3 overflow-hidden">
                        <div class="col-12">
                          <div class="form-floating mb-3">
                            <input type="email" class="form-control" id="email" placeholder="Email" name="email" required>
                            <label for="email" class="form-label">Email</label>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="form-floating mb-3">
                            <input type="password" class="form-control" id="motdepasse" placeholder="Password" name="motdepasse" required>
                            <label for="password" class="form-label">Password</label>
                          </div>
                        </div>

                        </div>
                        <div class="col-12">
                          <div class="d-grid">
                            <button class="btn btn-dark btn-lg" type="submit" name="log" >Se connecter</button>
                          </div>
                        </div>
                      </div>
                    </form>
                    <div class="row">
                      <div class="col-12">
                        <div class="d-flex gap-2 gap-md-4 flex-column flex-md-row justify-content-md-center mt-0">
                          <a href="register.php" class="link-secondary text-decoration-none">Inscription</a>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</form>
</body>
</html>