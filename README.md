
# ChromeLauncher
Application .net C# en ligne de commande qui permet d'automatiser le lancement de l'application Chrome sur un ou plusieurs écrans, en pointant sur différentes pages web.

## Commandes
<pre>
    **load** : Charge un fichier json d'actions … passer aux lanceurs de chrome, 
           avec en option le chemin du fichier. [-l, -load [-f=, -filepath=]]
   tests : Lance un nombre donn‚ de chromes sur autant d'‚cran, avec en option 
           le nombre d'instance. [-t, -tests [-n=, -nombre= (default=1, min=1,
           max=5)]]
 screens : Donne des informations sur les ‚crans disponibles. [-s, -screens]
    kill : Tue tous les processus chromes en cours d'‚xecution avant le 
           lancement. [-kill, -kill-all-chromes]
   delay : Ajoute un d‚lai d'attente, exprim‚ en milisecondes avant le
           lancement. [-delay=2500]
   debug : Active le debug lors de l'‚xecution. [-d, -debug]
   clear : Efface la sortie console. [-c, -clear]
 version : Donne la version & copyright de l'application. [-v, -version]
    help : Affiche cette aide. [-h, -? -help]
</pre>
