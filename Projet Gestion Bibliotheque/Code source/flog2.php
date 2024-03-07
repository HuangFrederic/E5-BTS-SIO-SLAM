<?php
session_start();
require 'connexion.php'; 

if(isset($_POST['log'])){  
    //verifie si l'email et mdp sont vides
    $email = !empty($_POST['email']) ? trim($_POST['email']) : null; //trim: enleve espace inutile, ?: operateur ternaire pour verif condition si ca sinon ci
    $passwordAttempt = !empty($_POST['motdepasse']) ? trim($_POST['motdepasse']) : null;
    
    $req = "SELECT * FROM user WHERE email = :email";
    $stmt = $pdo->prepare($req);
    
    $stmt->bindValue(':email', $email);
    $stmt->execute();
    $user = $stmt->fetch(PDO::FETCH_ASSOC);
    
    if($user === false){ //===: doit strictement etre ca
    echo '<script>alert("Email ou mot de passe incorrect"); window.location = "index.php";</script>';
    exit;
}
 else{
         
        $validPassword = password_verify($passwordAttempt, $user['motdepasse']);
        
        if($validPassword){
             
            $_SESSION['email'] = $user['email'];
            $_SESSION['id_user'] = $user['id_user'];
            $_SESSION['prenom'] = $user['prenom'];
            $_SESSION['nom'] = $user['nom'];
            header('location:menu.php');
            exit;
            
        } else{
            
        echo '<script>alert("Email ou mot de passe incorrecte"); window.location = "index.php";</script>';
        exit;
        }
    }
    }
?>

