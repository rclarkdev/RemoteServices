import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EmailSettings } from '../Types/email-settings';
import { FormBuilder, FormGroup, Validators, AbstractControl, FormControl } from '@angular/forms';

import Validation from '../utils/validation';
import { timeout } from 'rxjs';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css'],
})

export class ContactComponent implements OnInit {
  form: FormGroup = new FormGroup({
    fromname: new FormControl(''),
    email: new FormControl(''),
    message: new FormControl(''),
  });
  submitted = false;

  emailSettings: EmailSettings = {
    FromName: "",
    ToEmail: "",
    Subject: "",
    EmailBody: "",
    FromEmail: ""
  };

  contactSuccess: boolean = false;
  contactFailed: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group(
      {
        fromname: ['', Validators.required],
        message: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]]
      }
    );
  }
  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit(f: any): void {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }

    this.emailSettings.EmailBody = f['message'].value;
    this.emailSettings.FromName = f['fromname'].value;
    this.emailSettings.FromEmail = f['email'].value;
    this.emailSettings.Subject = "Customer Service";
    this.emailSettings.ToEmail = "postmaster@remoteservicesllc.com";

    const httpoptions = {
      headers: new HttpHeaders({ 'content-type': 'application/json' })
    }

    this.http.post<boolean>(this.baseUrl + 'email/send', this.emailSettings, httpoptions).subscribe(result => {

      if (result == true) {
        this.onReset();
        this.contactSuccess = true;
        setTimeout(() => {
          this.contactSuccess = false;
        }, 3000);
      }

    }, error => {
      this.contactFailed = true;
      setTimeout(() => {
        this.contactFailed = false;
      }, 3000);
    });
  }

  onReset(): void {
    this.submitted = false;
    this.form.reset();
  }
}
