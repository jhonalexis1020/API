import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  Usuarios: any = [];

  constructor(private http: HttpClient) { }


  ngOnInit(): void {
    this.GetUsers();
  }

  GetUsers(): void {
    this.http.get('http://localhost:5000/api/users')
      .subscribe(users => {
        this.Usuarios = users;
      });
  }


}
