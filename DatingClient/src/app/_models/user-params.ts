import { Gender } from "../_enums/gender";
import { User } from "./user";

export class UserParams {
    pageNumber: number = 1;
    pageSize: number = 5;
    gender: string;
    minAge: number = 18;
    maxAge: number = 100;
    orderBy: string = 'lastActive';

    constructor(user: User | null) {
        this.gender = user?.gender === Gender.Male ? Gender[Gender.Female] : Gender[Gender.Male];
    }
}