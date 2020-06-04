import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-hacker-news',
  templateUrl: './hacker-news.component.html'
})
export class HackerNewsComponent {
  public stories: Story[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<StoriesResponse>(baseUrl + 'News').subscribe(result => {
      this.stories = result.stories;
    }, error => console.error(error));
  }
}

interface StoriesResponse {
  stories: Story[];
  count: number;
}

interface Story {
  
}
