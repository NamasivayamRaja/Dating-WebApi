import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeago',
  standalone: true
})
export class TimeagoPipe implements PipeTransform {

  transform(value: Date | string) : string {
    if(!value) return '';
    const date = new Date(value);
    const now = new Date();
    const difference = now.getTime() - date.getTime(); // milliseconds difference
    if(difference < 1000) return 'just now';
    const seconds = Math.floor(difference / 1000);
    if(seconds < 60) return `${seconds} seconds ago`;
    const minutes = Math.floor(seconds / 60);
    if(minutes < 60) return `${minutes} minutes ago`;
    const hours = Math.floor(minutes / 60); 
    if(hours < 24) return `${hours} hours ago`;
    const days = Math.floor(hours / 24);
    if(days < 30) return `${days} days ago`;
    else if(days < 365) {
      const months = Math.floor(days / 30);
      return `${months} months ago`;
    }
    else {
      const years = Math.floor(days / 365);
      return `${years} years ago`;
    }
  }

}
