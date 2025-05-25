import { Photo } from "./Photo"

export interface Member {
  id: number
  userName: string
  age: number
  photoUrl: string
  knownAs: string
  gender: number
  introduction: string
  interests: string
  lookingFor: string
  createdDateTime: Date;
  lastActive: Date;
  city: string
  country: string
  photos: Photo[]
}
