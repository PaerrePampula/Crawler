using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MeleeMook : BaseMook
    {
    public override void DoAIThing()
    {
        base.DoAIThing();
        Debug.Log("Melee thing");
        //Vinkkejä/Rakenne-ehdotuksia leeville
        /*
         * Periaatteessa tää rakennehan tulee sille slime äijälle,
         * mut ota myös huomioon että jos me lisätään muita "melee mookkeja"
         * myöhemmin ajan riittävyyden tai vaihtelun vuoksi, muut melee äijät käyttäytyy
         * varmaan vähän eri tavalla kuin slime, niin harkitse sitä että 
         * slime tyypin skripti joko jaetaan vielä omaan komponenttiin josta haetaan
         * kyseisen vihollistyypin erikoiskäyttäytyminen tähän skriptiin (slimen tilanteessa
         * pieni venaus ja slimelle tyypillinen loikka) tai sitten inheritance rakenne,
         * Inheritance rakenteella koko järjestelmä olisi ehkä turhan vittumainen,
         * joten jos haluaa tehdä vähän skaalattavamman järjestelmän niin 
         * joku slimeBehaviour, josta haetaan jotain
         * slimen metodeja on periaatteessa kova, JOS haluaa rakentaa enemmänkin
         * melee tyyppisia mookkeja. Samapa se jos ei tee niin, tai jos slimen 
         * rakenne on tarpeeksi lähellä muita potenttiaalisia melee äijiä.
         * 
         * Loikkaan:
         * Hahmo voisi pysähtyä hetkeksi vaan paikalleen, odottaa jonkun
         * floatin tiedon perusteella x ajan, tai jonkun animaation loppuun.
         * Joku floatti vaan, attackStartTimer, ynnä muuta, ja sitten 
         * aloita joko CoRoutine jossa inkrementoit timeria, taikka laita se updateen
         * jossa se jonkun ehdon perusteella aina alkaa inkrementoimaan sitä. (+ resetoit tietenkin)
         * Nyt slime on loikkimassa niskaan, lasket vaan perus vektorimatematiikalla vektorin välistä
         * elikkäs käytännössä target - slimepos. Kerro se jollakin jos haluut vähän lisäetäisyyttä loikkaan.
         * sitten CoRoutinen avulla lerppaa slimen sijainti siihen targettiin, samankaltaisen
         * rakenteen löydät playerControllerista IENumeratoria returnaavalla metodilla
         * 
         * Tämän coroutinen sisälle täytys myös lisätä overlapSphere, jolle löydät ihan ok
         * esimerkin unityn dokumentaatiosta.
         * 
         * Kattele kokoajan siinä että onko osuneissa collidereissa objektia jonka 
         * tagi on player, tai joku muu mikä on asetettu pelaajan objektille.
         * Layermaskaa collider ainoastaan rekisteröimään pelaajan layermask parametrin kohdalla
         * Muuten hyökkäys castaa vasten kokoajan jotain ihan epäolennaisia collidereita ja vie performancea.
         * 
         * Jos sellanen löytyy, tee mikä lie damagefunktio sille keksit. Funktion saa melko helpolla
         * jos kutsut pelaajan pelaajaskriptista changehpta ja vähennät sitä haluamalla määrälläsi. Pitäisi onnistua jos getcomponenttaat hitistä IDamageable damageable = (IDamageable)GetComponent(typeof (IDamageAble));
         * 
         * Jos teet hyppy coroutinen erillisenä coroutinena hitbox coroutinesta, voit breakkaa
         * hitbox coroutinen aikasemmin, säästäen vähän performancea.
         */
    }

}

