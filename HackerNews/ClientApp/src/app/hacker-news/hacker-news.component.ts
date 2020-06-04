import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as _ from 'lodash';

@Component({
  selector: 'app-hacker-news',
  templateUrl: './hacker-news.component.html'
})
export class HackerNewsComponent {
  public stories: Story[];
  public page: number = 1;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<StoriesResponse>(baseUrl + 'News?count=25').subscribe(result => {
      this.stories = result.stories;

      http.get<StoriesResponse>(baseUrl + 'News?count=500').subscribe(result => {
        let allStories = result.stories;
        allStories = allStories.filter((story) => this.stories.indexOf(story) < 0);
        allStories = allStories.concat(this.stories);
        
        allStories = allStories.sort((a: Story, b: Story) => {
          return b.time - a.time;
        });
        
        this.stories = _.sortedUniqBy(allStories, 'id');
      })
      
    }, error => console.error(error));

    
  }
}

interface StoriesResponse {
  stories: Story[];
  count: number;
}

interface Story {
  id: number;
  title: string;
  by: string;
  time: number;
}
