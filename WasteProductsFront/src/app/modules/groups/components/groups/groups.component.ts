import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { GroupsService } from '../../services/groups.service';
import { AuthenticationService } from '../../../account/services/authentication.service';

import { GroupModel, GroupOfUserModel, GroupInfoModel } from '../../models/group';
import { MatDialog } from '@angular/material';
import { GroupDialogInfoComponent } from '../group-dialog-info/group-dialog-info.component';
import { ConfirmModel } from '../../models/confirm';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {

  private userId: string;
  private groupsSubject: BehaviorSubject<GroupOfUserModel[]> = new BehaviorSubject<GroupOfUserModel[]>([]);

  ownGroups: Observable<GroupOfUserModel[]> = this.groupsSubject.asObservable();
  otherGroups: Observable<GroupOfUserModel[]> = this.groupsSubject.asObservable();

  constructor(
    private authService: AuthenticationService,
    private groupsService: GroupsService,
    private dialog: MatDialog) {
  }

  ngOnInit() {
    this.userId = this.authService.getUserId();

    this.ownGroups = this.groupsSubject.pipe(map(groups => groups.filter(group => group.AdminId === this.userId)));
    this.otherGroups = this.groupsSubject.pipe(map(groups => groups.filter(group => !this.contains(this.ownGroups, group))));

    this.getGroups();
  }

  createGroup() {
    const dialogRef = this.dialog.open<GroupDialogInfoComponent, { action: string, data: GroupInfoModel }, GroupInfoModel>(
      GroupDialogInfoComponent, {
        // width: '250px',
        data: {
          action: 'Создать',
          data: {
            AdminId: this.userId,
            Name: 'Моя борда',
            Information: 'Немного о борде...'
          }
        }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.groupsService.createGroup(this.userId, result).subscribe(() => this.getGroups());
      }
    });
  }

  comeInToGroup() {

  }

  deleteGroup(groupId: string, event: Event) {
    this.onItemClick(event);

    const dialogRef = this.dialog.open<ConfirmDialogComponent, ConfirmModel, boolean>(
      ConfirmDialogComponent, {
        // width: '250px',
        data: {
          title: 'Подтвердите',
          question: 'Вы действительно хотите удалить группу?'
        }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.groupsService.deleteGroup(groupId).subscribe(() => this.getGroups());
      }
    });
  }

  leftGroup(groupId: string, event: Event) {
    this.onItemClick(event);

    const dialogRef = this.dialog.open<ConfirmDialogComponent, ConfirmModel, boolean>(
      ConfirmDialogComponent, {
        // width: '250px',
        data: {
          title: 'Подтвердите',
          question: 'Вы действительно хотите покинуть группу?'
        }
      });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.groupsService.leftGroup(groupId, this.userId).subscribe(() => this.getGroups());
      }
    });
  }


  private onItemClick(event: Event) {
    event.preventDefault();
    event.stopImmediatePropagation();
  }


  private getGroups() {
    this.groupsService.getUserGroups(this.userId).subscribe(groups => this.groupsSubject.next(groups));
  }

  private contains<T>(observable: Observable<Array<T>>, item: T): boolean {
    let result;
    observable.pipe(map(array => array.includes(item))).subscribe(r => result = r);
    return result;
  }


}
