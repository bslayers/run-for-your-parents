import bpy
import re

aPattern = "[Aa]"
bPattern = "[Bb]"
cPattern = "[Cc]"
fPattern = "[Ff]"
lPattern = "[Ll]"
rPattern = "[Rr]"
sPattern = "[Ss]"
tPattern = "[Tt]"
uPattern = "[Uu]"

chestPattern = fr"{cPattern}hest"
legPattern = fr"{lPattern}eg"
armPattern = fr"{aPattern}rm"
forarmPattern = fr"{fPattern}ore{armPattern}"
toesPattern = fr"{tPattern}oes?({bPattern}ase)?"


leftRightGroup = fr"({lPattern}eft|{rPattern}ight)"

# Assurez-vous que l'armature est sélectionnée
armature = bpy.context.object

# Passez en mode édition pour modifier les noms des os
bpy.ops.object.mode_set(mode='EDIT')

pattern_list = [
    (fr"{leftRightGroup}({uPattern}p{legPattern}|{uPattern}pper{legPattern}|{tPattern}high)",
     r"\1Thigh"),
    (fr"({sPattern}pine2|[Uu]pper{chestPattern}|[Uu]p{chestPattern})",
    r"UpperChest"),
    (fr"([Ss]pine1|{chestPattern})",
    r"Chest"),
    (fr"{leftRightGroup}{forarmPattern}",
    r"\1Forarm"),
    (fr"{leftRightGroup}{toesPattern}",
    r"\1Toes"),
]

for bone in armature.data.edit_bones:
    bone.name = re.sub(r"^mixamorig:(.*)",r"\1",bone.name).replace(" ", "")
    original_name = bone.name
    for pattern, replacement in pattern_list:
        new_name = re.sub(pattern, replacement, original_name)
        if new_name != original_name:
            bone.name = new_name
            break

# Revenez en mode objet
bpy.ops.object.mode_set(mode='OBJECT')