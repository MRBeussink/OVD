import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';

constructor(private http: HttpClient) { }

login(model: any) {
  // Call the api to log in with passed credentials
  return this.http.post(this.baseUrl + 'login', model).pipe(
    // Grab the token that was returned if the login was successful
    map((response: any) => {
      const user = response;
      // If there was a token, save it
      if (user) {
        localStorage.setItem('token', user.token);
      }
    })
  );
}

loggedIn() {
  const token = localStorage.getItem('token');
  return !!token;
}

logout() {
  localStorage.removeItem('token');
}

}
