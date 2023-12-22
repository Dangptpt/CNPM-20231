import { useEffect, useState } from "react";
import AutoComplete from "../../Layout/component/AutoCompleteSearch";
import { MenuItem, Select, TableCell, TableRow } from "@mui/material";
import { toast } from "react-toastify";
import ClassApi from '../../Api/Api'

import { NavLink } from "react-router-dom";
import { useListMember } from "./listMemberContext";

function CustomRow({ index, listMember, setListMember, changeRelation, personId }) {
    //  const { listMember, setListMember } = useListMember();
    const personShrinkList = [];
    const [name, setName] = useState()
    const [personList, setPersonList] = useState([])
    const [birth, setBirth] = useState('2003-02-11')
    const [identityCardNumber, setIdentityCardNumber] = useState('')
    const [relation, setRelation] = useState('Khác')
    const [id, setId] = useState('')
    useEffect(() => {
        ClassApi.GetAllPeople()
            .then((res) => {
                setPersonList(res.data);
            })
            .catch((err) => {
                toast.error("lỗi 1");
            });
    }, [name]);
    personList.map((person, index) => {
        personShrinkList.push({
            label: person.name,
            code: person.identityCardNumber,
            personId: person.personId,
            residenceId: person.residenceId,
            birth: person.dateOfBirth,
            person: person
        });
    });
    const handleDelete = () => {
        // Gọi hàm onDelete (deleterow) với index của phần tử cần xóa
        //ondelete()
        const updatedList = [...listMember];
        let list = []
        for (var i = 0; i < updatedList.length; i++) {
            if (updatedList[i].personId && updatedList[i].personId != personId) {
                list.push(updatedList[i])
            } else {
                console.log(updatedList[i])
            }

        }
        console.log(list)
        // Xóa phần tử tại vị trí index trong bản sao
        //   updatedList.splice(index, 1);
        // Cập nhật state với mảng mới đã xóa phần tử
        //    setListMember([])
        setListMember(list);
        console.log(updatedList.length)
        // console.log(listMember.length)
        //  let newlist = listMember.filter((item, index) => index !== index)
        //  setListMember(newlist)
    };
    // console.log(arr);
    const handleChangeName = (event, value) => {
        if (value != null && value.person != null) {
            var personn = value.person
            personn.ownerRelationship = relation
            setListMember([...listMember, value.person])
        }
    };
    const handleChangeBirth = (event) => {
        setBirth(event.target.value);
    };
    return (
        <TableRow key={index}>

            <TableCell>

                <AutoComplete
                    optionList={personShrinkList}
                    onChange={handleChangeName}
                    width={250}
                ></AutoComplete>
            </TableCell>
            <TableCell style={{ fontSize: "18px" }}>

            </TableCell>
            <TableCell style={{ fontSize: "18px" }}>
                <input
                    style={{
                        fontSize: "18px",
                        border: "none",
                        width: "150px",
                    }}
                    type="text"
                    value={identityCardNumber}
                    disabled
                //   onChange={(e)=>setIdentityCardNumber(e.target.value)}
                ></input>
            </TableCell>
            <TableCell style={{ fontSize: "18px" }}>

            </TableCell>
            <TableCell style={{ fontSize: "18px", cursor: "pointer" }}>
                <NavLink to={'/chitietcudan/' + id}>
                    <span style={{ color: "blue" }}></span>
                </NavLink>
                <button
                    style={{
                        backgroundColor: "transparent",
                        fontSize: "18px",
                    }}
                //      onClick={handleDelete}
                >
                    <span style={{ color: "red" }}></span>
                </button>
            </TableCell>
        </TableRow>
    );
}

export default CustomRow;