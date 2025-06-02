import { Gender } from "../_enums/gender";
import { Params } from "./params";
import { User } from "./user";

export class UserParams extends Params {
    gender: string;
    minAge: number = 18;
    maxAge: number = 100;
    orderBy: string = 'lastActive';

    constructor(user: User | null) {
        super();
        this.gender = user?.gender === Gender.Male ? Gender[Gender.Female] : Gender[Gender.Male];
    }
}