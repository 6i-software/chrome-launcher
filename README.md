# ChromeLauncher
Application .net C# en ligne de commande qui permet d'automatiser le lancement de l'application Chrome sur un ou plusieurs écrans, en pointant sur différentes pages web.

## Usages

    ChromeLauncher [commandes] [options]

**Commandes**
> `load` : Charge un fichier json d'actions à passer aux lanceurs de chrome, avec en option le chemin du fichier. `[-l, -load [-f=, -filepath=]]`

> `tests` : Lance un nombre donné de chromes sur autant d'écran. `[-t, -tests [-n=, -nombre= (default=1, min=1,max=5)]]`

> `screens` : Donne des informations sur les ãrans disponibles. `[-s, -screens]`

> `version` : Donne la version & copyright de l'application. `[-v, -version]`

> `help` : Affiche l'aide de l'application. `[-h, -? -help]`


**Options**
> `kill` : Tue tous les processus chromes en cours d'éxecution avant le lancement. `[-kill, -kill-all-chromes]`

> `delay` : Ajoute un délai d'attente, exprimé en milisecondes avant le lancement. `[-delay=2500]`

> `debug` : Active le debug lors de l'éxecution. `[-d, -debug]`

> `clear` : Efface la sortie console. `[-c, -clear]`


**Exemples**

```
> ChromeLauncher screens
```

<pre>
---- Screens informations ----

 Number screens detected = 2

 > SCREEN 1
   ---------------------------------------------------------
   |    Device Name : \\.\DISPLAY1
   |         Bounds : {X=0,Y=0,Width=1280,Height=1024}
   |           Type : System.Windows.Forms.Screen
   |   Working Area : {X=0,Y=0,Width=1280,Height=984}
   | Primary Screen : True
   ---------------------------------------------------------

 > SCREEN 2
   ---------------------------------------------------------
   |    Device Name : \\.\DISPLAY2
   |         Bounds : {X=1280,Y=0,Width=1920,Height=1200}
   |           Type : System.Windows.Forms.Screen
   |   Working Area : {X=1280,Y=0,Width=1920,Height=1200}
   | Primary Screen : False
   ---------------------------------------------------------

---- END ----
</pre>

## Historique
[Releases](https://github.com/AOT-PADI/ChromeLauncher/releases)

## License
[BSD License](https://github.com/AOT-PADI/ChromeLauncher/LICENSE.md)
