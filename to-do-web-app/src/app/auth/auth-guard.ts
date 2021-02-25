import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {
    isAuthenticated: boolean = false;

    constructor(private router: Router) {}

    canActivate(route: ActivatedRouteSnapshot): boolean {

        const expectedScopes = (route.data as any).expectedScopes.split(' ');

        if (!this.userHasScopes(expectedScopes)) {
            this.router.navigate(['']);
            return false;
        }
        return true; 
    }

    
    userHasScopes(expectedScopes: Array<string>): boolean {
        let storageScopes = localStorage.getItem('scopes');
        if (storageScopes) {
            const scopes = JSON.parse(storageScopes);           
            return expectedScopes.every(scope => scopes.includes(scope));
        }       
        return false; 
    }
  
}