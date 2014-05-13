
# ChromeLauncher
Application .net C# en ligne de commande qui permet d'automatiser le lancement de l'application Chrome sur un ou plusieurs écrans, en pointant sur différentes pages web.

## RoadMap
**Version 0.0.1**
* Affiche les informations des écrans disponibles;
* Lancement de chrome sur un ecran donnée, vers une url précise, avec des options en ligne de commande, avec un délai, et avec la possibilité de fermer au préalable toutes les instances de chromes;
* Lancement par lecture d'un fichier JSON;
* Lancement directement à l'aide d'un script bat (qui ne fait que lire une partition décrite dans un fichier JSON).

## Usages
<pre>
  ChromeLauncher screens
  ChromeLauncher tests
  ChromeLauncher tests -nombre=3 -debug -kill-all-chromes -delay=2500 -clear
  ChromeLauncher t -n=3 -d -kill -delay=2500 -c
  ChromeLauncher load -filepath=c:\foo\actions.json
  ChromeLauncher -l -f=actions.json -d -c
</pre>

## Commandes
Liste toutes les commandes disponibles
<pre>
    load : Charge un fichier json d'actions à passer aux lanceurs de chrome, 
           avec en option le chemin du fichier. [-l, -load [-f=, -filepath=]]
   tests : Lance un nombre donné de chromes sur autant d'écran, avec en option 
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
