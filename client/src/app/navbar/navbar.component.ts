import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  model: any = {};
  loggedIn: boolean = false;

  constructor(private accountSercices: AccountService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountSercices.login(this.model)
      .subscribe(response => {
        console.log(response);
        this.loggedIn = false;
      }, error => {
        console.log(error);
      });
  }

}
