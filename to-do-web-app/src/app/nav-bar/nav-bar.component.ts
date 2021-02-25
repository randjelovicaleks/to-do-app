import { Component, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { SearchService } from '../core/search.service';
import { AuthService } from '@auth0/auth0-angular';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  listTitle?: string;

  constructor(public router: Router, private searchServie: SearchService, 
              public auth: AuthService, @Inject(DOCUMENT) private doc: Document) { }

  ngOnInit(): void {
    
  }

  searchLists() {
    this.searchServie.searchLists(this.listTitle);
  }

  login() {
    this.auth.loginWithRedirect();
  }

  logout(): void {
    localStorage.removeItem('scopes');
    this.auth.logout({ returnTo: this.doc.location.origin });
  }

}
