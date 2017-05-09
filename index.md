### W2I ?
This application is a command line interface make with .net / C#.

It is able to launch on or multiple Chrome on a one given or multiple screen, and with another parameters (in fullscreen, with delay, given url ...). It was initialy created in order to used it in *digital signage* process. The goal of this process is to deliver targeted messages, to specific locations and/or consumers, at specific times. This is often called "digital out of home" (DOOH). 

It allows the automated launch Chrome on multiple screens by reading a Json file.![alt text](https://raw.githubusercontent.com/AOT-PADI/ChromeLauncher/gh-pages/images/ChromeLauncher-actions-json.png "ChromeLauncher actions.json")


### Download
Last setup (windows x86 - 32 bits) : [Setup_ChromeLauncher_v0.0.2](https://github.com/AOT-PADI/ChromeLauncher/releases/download/v0.0.2/Setup_ChromeLauncher_v0.0.2.exe)


### Usages
    ChromeLauncher [command] [option]

<br/>

| Command | Description
| :---: | --- 
| `load` | Use to read multiple actions stored in a Json file with one parameter, the filepath of the Json file. We call it *action.json*. <br/>`[l, load] [-f=, -filepath=]`
| `tests` | Run a given number tests. This command launch a scenario pre-defined on one or multiple screen. We can define with the parameter `number` the number of chrome to launch on the same number screen. With this method we can't specify any parameters (like opened url, fullscreen mode ...).<br/>`[t, tests] [-n=, -number= (default=1, min=1,max=5)]`
| `screens` | Give some information about available screens (number, size, primary screen ...).<br/>`[s, screens]`
| `version` | Give the version &amp; the copyright of this application.<br/>`[v, version]`
| `help` | Show the help in console.<br/>`[h, ?, help]`

<br/>

| Option | Description
| :---: | ---
| `kill` | Stop all chromes processus before the launching with `load` or `tests` command.<br/>`[-kill, -kill-all-chromes]`
| `delay` | Differ the launching in miliseconds.<br/>`[-delay=2500]`
| `debug` | Enable the debug (verbose mode).<br/>`[-d, -debug]`
| `clear` | Clean the output console before.<br/> `[-c, -clear]`


### Examples
```
\> ChromeLauncher screens
\> ChromeLauncher screens -clear
```

```
\> ChromeLauncher tests
\> ChromeLauncher tests -number=3 -debug -kill-all-chromes -delay=2500 -clear
\> ChromeLauncher t -n=3 -d -kill -delay=2500 -c
```

```
\> ChromeLauncher load -filepath=c:\foo\actions.json
\> ChromeLauncher l -f=actions.json -d -c
```

### Structure of "Actions" Json file 
Thereafter, we found an example of actions json file to load with ChromeLauncher. This file describe how the engine had to operate with many parameters.
<pre>
{
  "kill_all_chromes": true,
  "delay_before_launch":2500,
  "actions": [
    {
      "url":"http://20100.lescigales.org/FF1/",
      "arguments":"--new-window --incognito",
      "fullscreen": true,
      "indexscreen": 0
    },
    {
      "url":"http://20100.lescigales.org/FF1/solution/intro.php",
      "arguments":"--new-window --incognito",
      "fullscreen": true,
      "indexscreen": 1
    }
  ]
}
</pre>

* kill_all_chromes : Stop all chromes processus before the launching;
* delay_before_launch : Differ the launching in miliseconds;
* actions : Array which discribe an action with the followed parameters : 
    - url : Open chrome on this url
    - arguments : Open chrome with known argument's chrome;
    - fullscreen : Switches chrome to full screen (in the engine we simulate the F11 keypress);
    - indexscreen : If there are several screens, we can specifiy on which screen, we want to open the url. The 0 screen is generally the primary screen.


### Historique
[Releases](https://github.com/AOT-PADI/ChromeLauncher/releases)

### License
[BSD License](https://github.com/AOT-PADI/ChromeLauncher/blob/master/LICENSE)