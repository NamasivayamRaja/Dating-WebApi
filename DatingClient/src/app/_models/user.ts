import { Gender } from "../_enums/gender";

export interface User{
    userName: string;
    token: string;
    photoUrl?: string;
    knownAs?: String;
    gender?: Gender;
    roles?: string[];
}