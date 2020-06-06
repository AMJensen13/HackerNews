import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { HackerNewsComponent } from './hacker-news.component';
import { HttpClientTestingModule} from '@angular/common/http/testing';
import { NgxPaginationModule } from 'ngx-pagination';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { FormsModule } from '@angular/forms';
import { Story } from './hacker-news.component';

describe('HackerNewsComponent', () => {
    it('should create', () => {
      TestBed.configureTestingModule({
        imports: [ 
          HttpClientTestingModule,
          NgxPaginationModule,
          Ng2SearchPipeModule, 
          FormsModule,
        ],
        declarations: [ HackerNewsComponent ],
        providers:[{ provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }]
      });
      const fixture = TestBed.createComponent(HackerNewsComponent);
      const component = fixture.componentInstance;
      expect(component).toBeDefined();
    });

    it('should show stories', () => {
        TestBed.configureTestingModule({
          imports: [ 
            HttpClientTestingModule,
            NgxPaginationModule,
            Ng2SearchPipeModule, 
            FormsModule,
          ],
          declarations: [ HackerNewsComponent ],
          providers:[{ provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }]
        });
        const fixture = TestBed.createComponent(HackerNewsComponent);
        const component = fixture.componentInstance;
        component.stories = [ { id: 123, by: 'test', time: 123444444, title: 'test' } ];

        fixture.detectChanges(false);

        var test = fixture.debugElement.query((de) => {return de.nativeElement.class === 'font-weight-lighter'});
        
        expect(test).toBeDefined();
      });
})

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
  }