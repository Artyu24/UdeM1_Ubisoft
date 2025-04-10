using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundState
{
    UI_CLICK,
    BACKGROUND_MUSIC_NIVEAUX, //In each scene, add the AudioPrefab and toggle true the music you want to play since start of the map
    BACKGROUND_MUSIC_HUB,
    BACKGROUND_MUSIC_MENU,
    BACKGROUND_SFX_INSIDE_THE_STORE_AIRCLEMATISE, //IDK where to trigger this mdr
    SFX_VALVE, //
    SFX_PORTE, //
    SFX_PRESSURE_PLATE, //
    SFX_GOUTELETTE_EAU_VALVE, //
    SFX_EAU_QUI_TRAVERSE_LE_TUYAUX, //
    SFX_HUMAN_FOOTSTEPS, // Only on client IA, need to try
    SFX_HUMAN_GRUNT_HURT,
    SFX_RACOON_IDLE, //need to try
    SFX_RACOON_WALK, //need to try
    SFX_RACOON_HIT, //Hit != Interaction or Same ? -> Both for the moment
    SFX_EAU_QUI_TRAVERSE_LES_TUYAUX, //whats the dif btw this and the only one ?
    
    MUSIC
}